Set-StrictMode -version 2.0
$Global:PackageName = "socialsentiment"
$Global:DbPackage = "fulldatabase.zip"
$Global:NamePrefix="socialsenti"

Function Check-AzureRmModule
{
    #######################################################################
    #  Verify Azure PowerShell module and version
    #######################################################################

    # Import the Azure PowerShell module
    Write-Host "`n[WORKITEM] - Importing Azure PowerShell module" -ForegroundColor Yellow
    $azureModule = Import-Module Azure -PassThru

    if ($azureModule -ne $null)
    {
        Write-Host "`tSuccess" -ForegroundColor Green
    }
    else
    {
        # Show module not found interaction and bail out
        Write-Host "[ERROR] - PowerShell module not found. Exiting." -ForegroundColor Red
        Exit
    }

    # Check the Azure PowerShell module version
    Write-Host $azureModule.Version
    Write-Host "`n[WORKITEM] - Checking Azure PowerShell module verion" -ForegroundColor Yellow
    If ($azureModule.Version -ge (New-Object System.Version -ArgumentList "1.3.0"))
    {
        Write-Host "`tSuccess" -ForegroundColor Green
    }
    Else
    {
        Write-Host "[ERROR] - Azure PowerShell module must be version 1.3.0. Exiting." -ForegroundColor Red
        Exit
    }
}

Function Check-Net45
{
    if (Test-Path 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full')
    {
        if (Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full' -Name Release -ErrorAction SilentlyContinue)
        {

            Write-Host ".Net Framework checking passed." -ForegroundColor Green

        }
        else
        {
            Write-Host "[ERROR] - .Net framework checking failed, please install .Net Frameowrk 4.5" -ForegroundColor Red
        }
    }
    else
    {
        Write-Host "[ERROR] - .Net framework checking failed, please install .Net Frameowrk 4.5" -ForegroundColor Red
    }
}


Function Update-SqlAzureDatabase
{
    param
    (
       [parameter(Mandatory=$true)]
       $ResourceGroupName,
       [parameter(Mandatory=$true)]
       $ServerName,
       [parameter(Mandatory=$true)]
       $Location,
       [parameter(Mandatory=$true)]
       $DatabaseName, 
       [parameter(Mandatory=$true)]
       [string]$BlobUri,
       [parameter(Mandatory=$true)]
       $StorageAccountName,
       [parameter(Mandatory=$true)]
       $Credential,
       [parameter(Mandatory=$false)]
       [String]$StartIPAddress,
       [parameter(Mandatory=$false)]
       [String]$EndIPAddress
    )

    $server = Get-AzureRmSqlServer -ResourceGroupName $ResourceGroupName -ServerName $ServerName -ErrorAction SilentlyContinue

    if($server -eq $null)
    {
        If (-not ($StartIPAddress -and $EndIPAddress))
        {
            $ipRange = Detect-IPAddress
            $StartIPAddress = $ipRange.StartIPAddress
            $EndIPAddress = $ipRange.EndIPAddress
        }

        Check-ResourceGroup -resourceGroupName $ResourceGroupName -resourceGroupLocation $Location
        $server = New-AzureRmSqlServer -ResourceGroupName $ResourceGroupName -ServerName $ServerName -Location $Location -ServerVersion "12.0" -SqlAdministratorCredentials $Credential
        Write-Host "Database Server $ServerName Successfully Created" -ForegroundColor Green

        $rule = New-AzureRmSqlServerFirewallRule -ResourceGroupName $ResourceGroupName -ServerName $ServerName -FirewallRuleName "fwrAzure_$ServerName" -StartIpAddress "0.0.0.0" -EndIpAddress "0.0.0.0"
        Write-Host "Firewall rule to allow azure internal visit successfully created. " -ForegroundColor Green

        $rule = New-AzureRmSqlServerFirewallRule -ResourceGroupName $ResourceGroupName -ServerName $ServerName -FirewallRuleName "fwrClient_$ServerName" -StartIpAddress $StartIPAddress -EndIpAddress $EndIPAddress
        Write-Host "Firewall rule to allow client visit successfully created. " -ForegroundColor Green
    }
    else
    {
        $database = Get-AzureRmSqlDatabase -ServerName $ServerName -DatabaseName $DatabaseName  -ResourceGroupName $ResourceGroupName
        if($database -ne $null)
        {
            Write-Host "Pevious database exists, press Y for Delete"
            
            $delete = Read-Host
            if($delete -eq 'Y' -or $delete -eq 'y')
            {
                $rmdb = Remove-AzureRmSqlDatabase -ResourceGroupName $ResourceGroupName -ServerName $ServerName -DatabaseName $DatabaseName -Force -ErrorAction Stop
                $database = Get-AzureRmSqlDatabase -ServerName $ServerName -DatabaseName $DatabaseName  -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue
                while($database -ne $null)
                {
                    Start-Sleep 10
                    $database = Get-AzureRmSqlDatabase -ServerName $ServerName -DatabaseName $DatabaseName  -ResourceGroupName $ResourceGroupName
                }
           }
            else
            {
                exit
            }
        }
    }
    Write-Host "[WORKITEM] Start Importing database" -ForegroundColor Yellow
    $destStorageKey = (Get-AzureRmStorageAccountKey -ResourceGroupName $ResourceGroupName -Name $StorageAccountName).Value[0]
    $importRequest = New-AzureRmSqlDatabaseImport -ResourceGroupName $ResourceGroupName -ServerName $ServerName -DatabaseName $DatabaseName -StorageKeytype "StorageAccessKey" -StorageKey $destStorageKey  -StorageUri $BlobUri -AdministratorLogin $Credential.UserName -AdministratorLoginPassword $Credential.Password -Edition Standard -ServiceObjectiveName S1 -DatabaseMaxSizeBytes 500000000
    Write-Host $importRequest.OperationStatusLink
    $message = ''
    do
    {   try
        {     
            $status = Get-AzureRmSqlDatabaseImportExportStatus -OperationStatusLink $importRequest.OperationStatusLink
	    $message = $status.Status
            if($status.Status -ne 'Succeeded')
            {
                Write-Host "[WORKITEM] Checking database importing status: "$status.Status"," $status.StatusMessage -ForegroundColor Yellow
                Start-Sleep -Seconds 15
            }
        }
        catch
        {
            Write-Host "[WORKITEM] Checking database importing status meet Error, Wait and check next 15 seconds" -ForegroundColor Yellow
            Start-Sleep -Seconds 15
        }
        
    }
    while($message -ne 'Succeeded')

    Write-Host "[WORKITEM] Database Import completed: "$message -ForegroundColor Green
    $connstr = "Server=tcp:$ServerName.database.chinacloudapi.cn,1433;Database=$DatabaseName;User ID=$userName;Password=$password;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"
    return $connstr
}

Function New-PSCredentialFromPlainText
{
    Param(
        [String]$UserName,
        [String]$Password
    )

    $securePassword = ConvertTo-SecureString -String $Password -AsPlainText -Force
    Return New-Object System.Management.Automation.PSCredential($UserName, $securePassword)
}

Function Detect-IPAddress
{
    $ipregex = "(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)"
    $text = Invoke-RestMethod 'https://api.ipify.org/?format=json'
    $result = $null

    If($text -match $ipregex)
    {
        $ipaddress = $matches[0]
        $ipparts = $ipaddress.Split('.')
        $ipparts[3] = 0
        $startip = [string]::Join('.',$ipparts)
        $ipparts[3] = 255
        $endip = [string]::Join('.',$ipparts)

        $result = @{StartIPAddress = $startip; EndIPAddress = $endip}
    }

    Return $result
}

Function Exec-SqlScript
{
    param(
        [string] $scriptPath, 
        [string] $ConnectionString
    )

    $Conn=New-Object System.Data.SQLClient.SQLConnection $ConnectionString
    try
    {
        $sql = "EXEC sp_fulltext_table 'NewsStream', 'activate';`n"
        $sql = $sql+"EXEC sp_fulltext_table 'NewsStreamDeduplicated', 'activate'; `n"
        $sql = $sql+"EXEC sp_fulltext_table 'NewsStreamHourly', 'activate'; `n"
        $sql = $sql+"EXEC sp_fulltext_table 'NewsStream_BAT', 'activate'; `n"
        $sql = $sql+"EXEC sp_fulltext_table 'NewsStreamHourly_BAT', 'activate'; `n"


        if(![string]::IsNullOrEmpty($scriptPath))
        {
            $sql = Get-Content -Path $scriptPath;
        }
        $Conn.Open();

        $DataCmd = New-Object System.Data.SqlClient.SqlCommand;
        $DataCmd.CommandTimeout = 300;
        $DataCmd.CommandText = $sql
        $DataCmd.Connection = $Conn;
        $DataCmd.ExecuteNonQuery();

    }
    finally
    {
        $Conn.Close();
        $Conn.Dispose();
    }
}

Function Upload-DatabasePackage
{
    param
    (
        [parameter(Mandatory=$true)]
        $ResourceGroupName,
        [parameter(Mandatory=$true)]
        $StorageAccountName, 
        [parameter(Mandatory=$true)]
        $Location,
        [parameter(Mandatory=$true)]
        $Path
    )

    $destAccount = TryCreate-AzureBlobAccount -ResourceGroup $ResourceGroupName -SkuName 'Standard_LRS' -AccountName $StorageAccountName -Location $Location
    $destStorageKey = (Get-AzureRmStorageAccountKey -ResourceGroupName $ResourceGroupName -Name $destAccount.StorageAccountName).Value[0]
    $destContext = New-AzureStorageContext -StorageAccountName $destAccount.StorageAccountName -StorageAccountKey $destStorageKey
    $ContainerName = "database"
    $BlobName = $Global:PackageName
    $container =New-AzureStorageContainer -Name $ContainerName -Context $destContext -Permission Blob
    $ac = Set-AzureStorageBlobContent -File $Path -Container $ContainerName -Blob $BlobName -Context $destContext
    $blob = Get-AzureStorageBlob $BlobName -Container $ContainerName -Context $destContext
    $uri = $blob.ICloudBlob.Uri.AbsoluteUri.ToString()
    Write-Host "Database package successfully uploaded to storage account $uri" -ForegroundColor Green
    return $uri;
}

Function TryCreate-AzureBlobAccount($ResourceGroup, $AccountName, $SkuName,$Location)
{
    $group = Get-AzureRmResourceGroup -Name $ResourceGroup
    if($group -eq $null)
    {
        $group = New-AzureRmResourceGroup -Name $ResourceGroup -Location $Location
    }

    $storageAccount = Get-AzureRmStorageAccount -ResourceGroupName $ResourceGroup -Name $AccountName -ErrorAction SilentlyContinue
    if( $storageAccount -eq $null)
    {
        $storageAccount = New-AzureRmStorageAccount -ResourceGroupName $ResourceGroup -Name $AccountName  -Location $Location -Type $SkuName
    }

    return $storageAccount;
}



Check-AzureRmModule
Check-Net45

$mcprofile = Add-AzureRmAccount -EnvironmentName AzureChinaCloud
$path = Split-Path -Parent $PSCommandPath  

Write-Host "Please Input Resource Group Name For Storage Account" -ForegroundColor Cyan 
$resourceGroupName = Read-Host

Write-Host "Please input the user name for sql database, sa is not allowed" -ForegroundColor Yellow
$userName = Read-Host

Write-Host "Please input the password for sql database" -ForegroundColor Yellow
$password = Read-Host
$credential = New-PSCredentialFromPlainText -UserName $userName -Password $password

$rg = Get-AzureRmResourceGroup -Name $resourceGroupName

$storageAccountName = $resourceGroupName.Replace("RG", "sa")
$dbServerName = $resourceGroupName.Replace("RG", "db") 
$Location = $rg.Location

$dbName = $Global:PackageName
$uri = Upload-DatabasePackage -ResourceGroupName $resourceGroupName -StorageAccountName $storageAccountName -Location $Location -Path $path"\"$Global:DbPackage
$connStr = Update-SqlAzureDatabase -ResourceGroupName $resourceGroupName -ServerName $dbServerName -Location $Location -DatabaseName $dbName -StorageAccountName $storageAccountName -BlobUri $uri -Credential $credential
Exec-SqlScript -ConnectionString $connStr
Write-Host "Successfully updated the database" -ForegroundColor Green