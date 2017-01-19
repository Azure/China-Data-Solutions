Write-Verbose "Running functions script at $PSCommandPath"

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
		Write-Host "Using existing resource group '$resourceGroupName'" -ForegroundColor Yellow
	}
}

Function Create-SqlAzureDatabase
{
	param
	(
	   [parameter(Mandatory=$true)]
	   $resourceGroupName,
	   [parameter(Mandatory=$true)]
	   $databaseServerName,
	   [parameter(Mandatory=$true)]
	   $location,
	   [parameter(Mandatory=$true)]
	   $databaseName,
	   [parameter(Mandatory=$true)]
	   $databaseUserName,
	   [parameter(Mandatory=$true)]
	   $databasePassword
	)

	$ipRange = Detect-IPAddress
	$startIPAddress = $ipRange.StartIPAddress
	$sndIPAddress = $ipRange.EndIPAddress

	$securePassword = ConvertTo-SecureString -String $databasePassword -AsPlainText -Force
	$credential = New-Object System.Management.Automation.PSCredential($databaseUserName, $securePassword)

	$server = Get-AzureRmSqlServer -ResourceGroupName $resourceGroupName -ServerName $databaseServerName -ErrorAction SilentlyContinue
	if (!$server) {
		$server = New-AzureRmSqlServer -ResourceGroupName $resourceGroupName -ServerName $databaseServerName -Location $location -ServerVersion "12.0" -SqlAdministratorCredentials $credential
		Write-Host "Database Server $databaseServerName Successfully Created" -ForegroundColor Green
	}
	else {
		Write-Host "Database Server $databaseServerName already existed" -ForegroundColor Yellow
	}

	$azureInternalRuleName = "fwrAzure_$databaseServerName"
	$azureInternalRule = New-AzureRmSqlServerFirewallRule -ResourceGroupName $resourceGroupName -ServerName $databaseServerName -FirewallRuleName $azureInternalRuleName -StartIpAddress "0.0.0.0" -EndIpAddress "0.0.0.0" -ErrorAction SilentlyContinue
	Write-Host "Firewall rule to allow azure internal visit successfully created. " -ForegroundColor Green

	$azureClientRuleName = "fwrClient_$databaseServerName"
	$rule = New-AzureRmSqlServerFirewallRule -ResourceGroupName $resourceGroupName -ServerName $databaseServerName -FirewallRuleName $azureClientRuleName -StartIpAddress $startIPAddress -EndIpAddress $sndIPAddress -ErrorAction SilentlyContinue
		Write-Host "Firewall rule to allow client visit successfully created. " -ForegroundColor Green

	$database = Get-AzureRmSqlDatabase -ResourceGroupName $resourceGroupName -ServerName $databaseServerName -DatabaseName $databaseName -ErrorAction SilentlyContinue
	if (!$database) {
		$database = New-AzureRmSqlDatabase -ResourceGroupName $resourceGroupName -ServerName $databaseServerName -DatabaseName $databaseName
		Write-Host "Database successfully created. " -ForegroundColor Green
	} else {
		Write-Host "Database already existed. " -ForegroundColor Yellow
	}

	$connstr = "Server=tcp:$databaseServerName.database.chinacloudapi.cn,1433;Database=$databaseName;User ID=$databaseUserName;Password=$databasePassword;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"
	return $connstr
}

Function Exec-SqlCommandText
{
	param(
		[parameter(Mandatory=$true)]
        [string]
		$commandText,
        [parameter(Mandatory=$true)]
		[string]
		$connectionString
    )

	$conn = New-Object System.Data.SQLClient.SQLConnection $connectionString
    try
    {
        $conn.Open();

        $cmd = New-Object System.Data.SqlClient.SqlCommand;
        $cmd.CommandTimeout = 300;
        $cmd.CommandText = $commandText
        $cmd.Connection = $conn;
        $result = $cmd.ExecuteNonQuery();
    }
    finally
    {
        $conn.Close();
        $conn.Dispose();
    }
}

Function Exec-SqlScript
{
    param(
		[parameter(Mandatory=$true)]
        [string]
		$scriptPath,
        [parameter(Mandatory=$true)]
		[string]
		$connectionString
    )

	Write-Host "SQL script $scriptPath executing..." -ForegroundColor Gray

	$commandTexts = [System.IO.File]::ReadAllText($scriptPath).Split([string[]]@("GO"), [System.StringSplitOptions]::RemoveEmptyEntries)

	foreach ($commandText in $commandTexts)
	{
		Exec-SqlCommandText -connectionString $connectionString -commandText $commandText
	}

	Write-Host "SQL script $scriptPath execution succeed." -ForegroundColor Green
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
		$plan = New-AzureRmAppServicePlan -ResourceGroupName $resourceGroupName -Name $servicePlanName -Location $location -NumberofWorkers  $numofWorkers -Tier $tier
		Write-Host "Service Plan $servicePlanName Created" -ForegroundColor Green
	}
	else{
		Write-Host "Using existing service plan '$servicePlanName'";
	}

	return $plan
}

Function Create-AzureWebApp{
    param
    (
       [parameter(Mandatory=$true)]
	   [string]
       $resourceGroupName,
       [parameter(Mandatory=$true)]
	   [string]
       $webAppName,
       [parameter(Mandatory=$true)]
	   [string]
       $location,
       [parameter(Mandatory=$true)]
	   [string]
       $servicePlanName,
	   [parameter(Mandatory=$true)]
	   [string]
       $profilePath
    )

	$webApp = New-AzureRmWebApp -ResourceGroupName $resourceGroupName -Name $webAppName -Location $location -AppServicePlan $servicePlanName -ErrorAction Stop
	Write-Host "Web App $webAppName successfully created." -ForegroundColor Green

    $profile = Get-AzureRmWebAppPublishingProfile -WebApp $webApp -OutputFile $profilePath -ErrorAction Stop
    Write-Host "Published Profile Downloaded" -ForegroundColor Green

	[System.Xml.XmlDocument]$xml = New-Object System.Xml.XmlDocument
    $xml.LoadXml($profile)
    $node = $xml.SelectSingleNode("//publishProfile[@publishMethod='FTP']")

    $url = $node.Attributes["publishUrl"].Value
    $mode = [System.Convert]::ToBoolean($node.Attributes["ftpPassiveMode"].Value)
    $user = $node.Attributes["userName"].Value
    $password = $node.Attributes["userPWD"].Value

    Upload-Directory -url $url -mode $mode -user $user -password $password -path $AppServiceHostPath -ErrorAction Stop
    Write-Host "Files successfully uploaded to FTP" -ForegroundColor Green

    return $webApp
}

Function Upload-Directory
{
    param(
        [parameter(Mandatory=$true)]
        [String]
        $url,
		[parameter(Mandatory=$true)]
		[Boolean]
		$mode,
		[parameter(Mandatory=$true)]
		[String]
        $user,
		[parameter(Mandatory=$true)]
		[String]
        $password,
		[parameter(Mandatory=$true)]
        [String]
        $path
    )

    $result = CheckAndMake-FTPDirectory -url $url -mode $mode -user $user -password $password -ErrorAction Stop

    foreach ($file in (Get-ChildItem -Path $path -File))
    {
		$subUrl = "$url/$($file.Name)"

        Write-Host "Uploading File $file" -ForegroundColor Gray
        Upload-File -url $subUrl -mode $mode -user $user -password $password -path $file.FullName
    }

    foreach ($dir in (Get-ChildItem -Path $path -Directory))
    {
        $subUrl = "$url/$($dir.Name)"
        $subPath = "$path/$($dir.Name)"

        Write-Host "Uploading Directory $subPath" -ForegroundColor Gray
        Upload-Directory -url $subUrl -mode $mode -user $user -password $password -path $subPath
    }
}

Function Upload-File
{
    param(
        [parameter(Mandatory=$true)]
        [String]
        $url,
		[parameter(Mandatory=$true)]
		[Boolean]
		$mode,
		[parameter(Mandatory=$true)]
		[String]
        $user,
		[parameter(Mandatory=$true)]
		[String]
        $password,
		[parameter(Mandatory=$true)]
        [String]
        $path
    )
    $suc = $false
    $retryTimes = 0
    while($suc -eq $false -and $retryTimes -lt 3)
    {
        try
        {
            $webclient = New-Object System.Net.WebClient
            $webclient.Credentials = New-Object System.Net.NetworkCredential($user,$password)
            $webclient.UploadFile($url, $path)
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
            [Boolean]
            $mode,
            [parameter(Mandatory=$true)]
            [String]
            $user,
            [parameter(Mandatory=$true)]
            [String]
            $password
        )

     try
     {
	    $makeDirectory = [System.Net.WebRequest]::Create($url);
        $makeDirectory.UsePassive = $mode
	    $makeDirectory.Credentials = New-Object System.Net.NetworkCredential($user,$password);
	    $makeDirectory.Method = [System.Net.WebRequestMethods+FTP]::MakeDirectory;
	    $makeDirectory.GetResponse();
        Write-Host "Directory on FTP $url Created"

     }
     catch [Net.WebException]
     {
	    try {

		    #if there was an error returned, check if folder already existed on server
		    $checkDirectory = [System.Net.WebRequest]::Create($url);
		    $checkDirectory.Credentials = New-Object System.Net.NetworkCredential($user,$password);
		    $checkDirectory.Method = [System.Net.WebRequestMethods+FTP]::PrintWorkingDirectory;
		    $response = $checkDirectory.GetResponse();

		    #folder already exists!
	    }
	    catch [Net.WebException] {
		    Write-Host "Error while connecting the FTP when creating folder $url"
	    }
    }
}
