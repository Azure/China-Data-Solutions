﻿Set-StrictMode -version 3.0
$Global:PackageName = "Web"
$Global:NamePrefix="sscn"
$Global:DbPackage = "sampledatabase.zip"

class FTPInfo
{
    [string]$Url
    [string]$User
    [string]$Password
    [bool]$PassiveMode
}

Function ParseFtp([string]$profile)
{
    [FTPInfo]$ftpInfo = New-Object FTPInfo
    [System.Xml.XmlDocument]$xml =  New-Object System.Xml.XmlDocument
    $xml.LoadXml($profile)
    $node = $xml.SelectSingleNode("//publishProfile[@publishMethod='FTP']") 
    $url = $node.Attributes["publishUrl"].Value
    $mode = $node.Attributes["ftpPassiveMode"].Value
    $user = $node.Attributes["userName"].Value
    $password = $node.Attributes["userPWD"].Value
    $ftpInfo.Url = $url
    $ftpInfo.User = $user
    $ftpInfo.Password = $password
    $ftpInfo.PassiveMode = [System.Convert]::ToBoolean($mode)

    return $ftpInfo
}

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


Function Check-ResourceGroup($resourceGroupName, $resourceGroupLocation)
{
    $resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
    if(!$resourceGroup)
    {
        if(!$resourceGroupLocation) {
            $resourceGroupLocation = Read-Host "resourceGroupLocation";
        }
        Write-Host "Creating resource group '$resourceGroupName' in location '$resourceGroupLocation'";
        $rg = New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation

        Write-Host "Resource Group $resourceGroupName Created" -ForegroundColor Green
    }
    else{
        Write-Host "Using existing resource group '$resourceGroupName'";
    }
}

Function Check-ServicePlan
{
    param(
        [parameter(Mandatory=$true)]
        $resourceGroupName, 
        [parameter(Mandatory=$true)]
        $servicePlanName, 
        [parameter(Mandatory=$true)]
        $location, 
        [parameter(Mandatory=$false)]
        $tier = 'Standard', 
        [parameter(Mandatory=$false)]
        $numofWorkers=5)

    $plan  = Get-AzureRmAppServicePlan -ResourceGroupName $resourceGroupName -Name $servicePlanName -ErrorAction SilentlyContinue
    if(!$plan)
    {
        if(!$location) {
            $resourceGroupLocation = Read-Host "resourceGroupLocation";
        }
        $sp = New-AzureRmAppServicePlan -ResourceGroupName $resourceGroupName -Name $servicePlanName -Location $location -NumberofWorkers  $numofWorkers -Tier $tier
        Write-Host "Service Plan $servicePlanName Created" -ForegroundColor Green
    }
    else{
        Write-Host "Using existing service plan '$servicePlanName'";
    }
}

Function Upload-Directory
{
    param(
        [parameter(Mandatory=$true)]
        [FTPInfo]
        $ftp, 
        [parameter(Mandatory=$true)]
        [String]
        $path
    )

    $result = CheckAndMake-FTPDirectory -url $ftp.Url -ftpuname $ftp.User -ftppassword $ftp.Password -passiveMode $ftp.PassiveMode -ErrorAction Stop
    $dirs = Get-ChildItem -Path $path -Directory
    $files = Get-ChildItem -Path $path -File

    foreach($file in $files)
    {
        [FTPInfo]$subFtp = New-Object $ftp
        $subFtp.PassiveMode = $ftp.PassiveMode
        $subFtp.Url = $ftp.Url+"\"+ $file.Name
        $subFtp.User = $ftp.User
        $subFtp.Password = $ftp.Password
        Write-Host "Uploading File $file"
        Upload-File -ftp $subFtp -path $file.FullName
    }

    foreach($dir in $dirs)
    {
        [FTPInfo]$subFtp = New-Object $ftp
        $subFtp.PassiveMode = $ftp.PassiveMode
        $subFtp.Url = $ftp.Url+"\"+ $dir.Name
        $subFtp.User = $ftp.User
        $subFtp.Password = $ftp.Password
        $subPath = $path+ '\' + $dir.Name
        Write-Host "Uploading Directory $subPath"
        Upload-Directory -ftp $subFtp -path $subPath
    }
}

Function Upload-File([FTPInfo]$ftp, $path)
{
    $suc = $false
    $retryTimes = 0
    while($suc -eq $false -and $retryTimes -lt 3)
    {
        try
        {
            $webclient = New-Object System.Net.WebClient 
            $webclient.Credentials = New-Object System.Net.NetworkCredential($ftp.User,$ftp.Password) 
            $webclient.UploadFile($ftp.Url, $path)  
             $suc = $true  
        }
        catch
        {
            $retryTimes = $retryTimes+1
        }
    }    

    Write-Host "Succeessfully Uploaded the file $path".
}

Function CheckAndMake-FTPDirectory{
    param(
            [parameter(Mandatory=$true)]
            [String]
            $url, 
            [parameter(Mandatory=$true)]
            [String]
            $ftpuname,
            [parameter(Mandatory=$true)]
            [String]
            $ftppassword,
            [parameter(Mandatory=$true)]
            [String]
            $passiveMode
        )

     try
     {
	    $makeDirectory = [System.Net.WebRequest]::Create($url);
        $makeDirectory.UsePassive = $passiveMode
	    $makeDirectory.Credentials = New-Object System.Net.NetworkCredential($ftpuname,$ftppassword);
	    $makeDirectory.Method = [System.Net.WebRequestMethods+FTP]::MakeDirectory;
	    $makeDirectory.GetResponse();
        Write-Host "Directory on FTP $url Created"
 
     }
     catch [Net.WebException] 
     {
	    try {
 
		    #if there was an error returned, check if folder already existed on server
		    $checkDirectory = [System.Net.WebRequest]::Create($url);
		    $checkDirectory.Credentials = New-Object System.Net.NetworkCredential($ftpuname,$ftppassword);
		    $checkDirectory.Method = [System.Net.WebRequestMethods+FTP]::PrintWorkingDirectory;
		    $response = $checkDirectory.GetResponse();
 
		    #folder already exists!
	    }
	    catch [Net.WebException] {				
		    Write-Host "Error while connecting the FTP when creating folder $url"
	    }	
    }
}

Function Create-AzureWebApp{
    param
    (
       [parameter(Mandatory=$true)]
       $ResourceGroupName,
       [parameter(Mandatory=$true)]
       $WebAppName,
       [parameter(Mandatory=$true)]
       $Location,
       [parameter(Mandatory=$true)]
       $ServicePlanName
    )


    Check-ResourceGroup -resourceGroupName $ResourceGroupName -resourceGroupLocation $Location
    Check-ServicePlan -resourceGroupName $ResourceGroupName -servicePlanName $ServicePlanName -location $Location

    $profileFolder = New-Item -ItemType directory -Path "$Env:TEMP\$ResourceGroupName" -Force
    $profilePath = $Env:TEMP + "\" + $ResourceGroupName + "\" + $WebAppName + "_src.json"
    $webApp = New-AzureRmWebApp -ResourceGroupName $ResourceGroupName -Name $WebAppName -Location $Location -AppServicePlan $ServicePlanName -ErrorAction Stop
    Write-Host "Web App $WebAppName successfully created" -ForegroundColor Green

    $profile = Get-AzureRmWebAppPublishingProfile -WebApp $webApp -OutputFile $profilePath -ErrorAction Stop
    Write-Host "Published Profile Downloaded" -ForegroundColor Green

    [FTpInfo]$uploadInfo = ParseFtp -profile $profile
    Write-Host "FTP Connection Successfully Parsed" -ForegroundColor Green

    $path = Split-Path -Parent $PSCommandPath  
    $contentPath = "$path\$Global:PackageName"

    Upload-Directory -ftp $uploadInfo -path $contentPath -ErrorAction Stop
    Write-Host "Files successfully uploaded to FTP" -ForegroundColor Green

    return $webApp
}


Function Create-SqlAzureDatabase
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
       $StorageAccountName,
       [parameter(Mandatory=$true)]
       $Credential,
       [parameter(Mandatory=$false)]
       [String]$StartIPAddress,
       [parameter(Mandatory=$false)]
       [String]$EndIPAddress
    )

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

    $database = New-AzureRmSqlDatabase -ResourceGroupName $ResourceGroupName -ServerName $ServerName -DatabaseName $DatabaseName

    Write-Host "[WORKITEM] Database Import completed: "$status.Status -ForegroundColor Green
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

Function CreateEventHub
{
    Param(
        [string] [Parameter(Mandatory=$true)] $ResourceGroupLocation,
        [string] $ResourceGroupName,
        [string] $TemplateFile = '.\Eventhub\azuredeploy.json',
        [string] $TemplateParametersFile = '.\Eventhub\azuredeploy.parameters.json'
    )

$OptionalParameters = New-Object -TypeName Hashtable
$TemplateFile = [System.IO.Path]::Combine($PSScriptRoot, $TemplateFile)
$TemplateParametersFile = [System.IO.Path]::Combine($PSScriptRoot, $TemplateParametersFile)

New-AzureRmResourceGroupDeployment -Name ((Get-ChildItem $TemplateFile).BaseName + '-' + ((Get-Date).ToUniversalTime()).ToString('MMdd-HHmm')) `
                                   -ResourceGroupName $ResourceGroupName `
                                   -TemplateFile $TemplateFile `
                                   -TemplateParameterFile $TemplateParametersFile `
                                   @OptionalParameters `
                                   -Force -Verbose
}

Function CreateASAJob
{
Param(
    [string] [Parameter(Mandatory=$true)] $ResourceGroupLocation,
    [string] $ResourceGroupName = 'Azure.ARM.EventHub',
    [string] $TemplateFile = '.\ASA\azuredeploy.json',
    [string] $TemplateParametersFile = '.\ASA\azuredeploy.parameters.json'
)

$OptionalParameters = New-Object -TypeName Hashtable
$TemplateFile = [System.IO.Path]::Combine($PSScriptRoot, $TemplateFile)
$TemplateParametersFile = [System.IO.Path]::Combine($PSScriptRoot, $TemplateParametersFile)

New-AzureRmResourceGroupDeployment -Name ((Get-ChildItem $TemplateFile).BaseName + '-' + ((Get-Date).ToUniversalTime()).ToString('MMdd-HHmm')) `
                                   -ResourceGroupName $ResourceGroupName `
                                   -TemplateFile $TemplateFile `
                                   -TemplateParameterFile $TemplateParametersFile `
                                   @OptionalParameters `
                                   -Force -Verbose
}

Function TryCreate-AzureStorageAccount($ResourceGroup, $AccountName, $SkuName,$Location)
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

#$mcprofile = Add-AzureRmAccount -EnvironmentName AzureChinaCloud
$Location = 'China North'
$selection = '0'

while($selection -ne '1' -or $selection -ne '2')
{
    Write-Host "Please select the deployment location [1] China North [2] China East"
    $selection = Read-Host
    if($selection -eq '1' -or $selection -eq '2')
    {
        if($selection -eq '2')
        {
            $Location = 'China East'
        }
        break;
    }
}

$postfix = [System.Environment]::TickCount.ToString()
$resourceGroupName = "RG$Global:NamePrefix$postfix"
$servicePlanName = "SP$Global:NamePrefix$postfix"
$storageAccountName = "sa$Global:NamePrefix$postfix"
$webAppName = "WS$Global:NamePrefix$postfix"
$dbServerName = "db$Global:NamePrefix$postfix"
$dbName = $Global:PackageName

$connStr=""
if([string]::IsNullOrEmpty($connStr))
{
    Write-Host "Please input the user name for sql database, sa is not allowed" -ForegroundColor Yellow
    $userName = Read-Host 

    Write-Host "Please input the password for sql database" -ForegroundColor Yellow
    $password = Read-Host

    $credential = New-PSCredentialFromPlainText -UserName $userName -Password $password
}

$path = Split-Path -Parent $PSCommandPath  
Check-ResourceGroup -resourceGroupName $resourceGroupName -resourceGroupLocation $Location

$account = TryCreate-AzureStorageAccount -ResourceGroup $ResourceGroupName -SkuName 'Standard_LRS' -AccountName $StorageAccountName -Location $Location
$key = (Get-AzureRmStorageAccountKey -ResourceGroupName $ResourceGroupName -Name $account.StorageAccountName).Value[0]
$accoutConnection = "BlobEndpoint=http://{0}.blob.{2}/;QueueEndpoint=http://{0}.queue.{2}/;TableEndpoint=http://{0}.table.{2}/;AccountName={0};AccountKey={1}" -f $account.StorageAccountName, $key, $account.Context.EndPointSuffix

$result = createeventhub -ResourceGroupName $resourceGroupName -ResourceGroupLocation $Location
$eventhubConnStr = $result.Outputs["namespaceDefaultConnectionString"].Value
$eventhubNS = $result.Outputs["namespaceName"].Value

if([string]::IsNullOrEmpty($connStr))
{
    $connStr = Create-SqlAzureDatabase -ResourceGroupName $resourceGroupName -ServerName $dbServerName -Location $Location -DatabaseName $dbName -StorageAccountName $storageAccountName -Credential $credential
	Exec-SqlScript -ConnectionString $connStr -scriptPath $path\initDB.sql
}


(Get-Content "$path\ASA\azuredeploy.json").Replace("{{EventHubNS}}",$eventhubNS).Replace("{{SqlServerName}}",$dbServerName).Replace("{{SqlServerUser}}",$userName).Replace("{{SqlServerPassword}}",$password).Replace("{{DBName}}",$dbName) | Set-Content "$path\ASA\azuredeployASA.json"
$result = CreateASAJob -ResourceGroupName $resourceGroupName -ResourceGroupLocation $Location -TemplateFile "$path\ASA\azuredeployASA.json"
Remove-Item "$path\ASA\azuredeployASA.json"

(Get-Content "$path\$Global:PackageName\\app_data\jobs\continuous\DataCollectingJob\DataCollectingJob.exe.config.tpl").Replace('[[StorageConnection]]', $accoutConnection).Replace("[[EventHubConnection]]", $eventhubConnStr) | Set-Content "$path\$Global:PackageName\app_data\jobs\continuous\DataCollectingJob\DataCollectingJob.exe.config"
(Get-Content "$path\$Global:PackageName\\app_data\jobs\continuous\NewsTextAnalysisJob\NewsTextAnalysisJob.exe.config.tpl").Replace('[[StorageConnection]]', $accoutConnection).Replace("[[DatabaseConnection]]", $connStr) | Set-Content "$path\$Global:PackageName\app_data\jobs\continuous\NewsTextAnalysisJob\NewsTextAnalysisJob.exe.config"
Create-AzureWebApp -ResourceGroupName $resourceGroupName -WebAppName $webAppName -ServicePlanName $servicePlanName -Location $Location
$webApp = Get-AzureRmWebApp -ResourceGroupName $resourceGroupName -Name $webAppName
Write-Host $webApp.HostNames -ForegroundColor Green