<#
 .SYNOPSIS
    Deploys a template to Azure

 .DESCRIPTION
    Deploys an Azure Resource Manager template

 .PARAMETER subscriptionId
    The subscription id where the template will be deployed.

 .PARAMETER resourceGroupName
    The resource group where the template will be deployed. Can be the name of an existing or a new resource group.

 .PARAMETER resourceGroupLocation
    Optional, a resource group location. If specified, will try to create a new resource group in this location. If not specified, assumes resource group is existing.

 .PARAMETER deploymentName
    The deployment name.

 .PARAMETER templateFilePath
    Optional, path to the template file. Defaults to template.json.

 .PARAMETER parametersFilePath
    Optional, path to the parameters file. Defaults to parameters.json. If file is not found, will prompt for parameter values based on template.
#>

param(
 [string]
 $subscriptionId="40ad315e-87c4-45e8-a907-c8cd123571e2",

 [string]
 $resourceGroupName ="RServerTest1",

 [string]
 $resourceGroupLocation,

 [string]
 $VMName="rservertest1827",

 [string]
 $VMPassword="Passw0rd123!",

 [string]
 $deploymentName,

 [string]
 $templateFilePath = "template.json",

 [string]
 $parametersFilePath = "parameters.json"
)

$Global:PackageName = "SNADemo"

<#
.SYNOPSIS
    Registers RPs
#>
Function RegisterRP {
    Param(
        [string]$ResourceProviderNamespace
    )

    Write-Host "Registering resource provider '$ResourceProviderNamespace'";
    Register-AzureRmResourceProvider -ProviderNamespace $ResourceProviderNamespace;
}

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

#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************
$ErrorActionPreference = "Stop"

Check-AzureRmModule
Check-Net45

# sign in
Write-Host "Logging in...";
#Login-AzureRmAccount -EnvironmentName AzureChinaCloud;

# select subscription
Write-Host "Selecting subscription '$subscriptionId'";
Select-AzureRmSubscription -SubscriptionID $subscriptionId;

$path = Split-Path -Parent $PSCommandPath  
$dbName = "SNADemo"
$dbServerName = $VMName+"dbserver"
$siteName = $VMName + "sites"
$storageName = $VMName+"dstor"
$ipName = $VMName + "-ip"

$ipRange = Detect-IPAddress
$StartIPAddress = $ipRange.StartIPAddress
$EndIPAddress = $ipRange.EndIPAddress
#$rule = New-AzureRmSqlServerFirewallRule -ResourceGroupName $resourceGroupName -ServerName $dbServerName -FirewallRuleName "fwrClient_$dbServerName" -StartIpAddress $StartIPAddress -EndIpAddress $EndIPAddress

$dbConnection = "Server=tcp:$dbServerName.database.chinacloudapi.cn,1433;Database=$dbName;User ID=dbuser;Password=$VMPassword;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"
#Exec-SqlScript -ConnectionString $dbConnection -scriptPath $path\initDB.sql
$account = Get-AzureRmStorageAccount -ResourceGroupName $resourceGroupName -Name $storageName
$key = (Get-AzureRmStorageAccountKey -ResourceGroupName $resourceGroupName -Name $account.StorageAccountName).Value[0]
$accoutConnection = "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};EndpointSuffix={2};" -f $account.StorageAccountName, $key, $account.Context.EndPointSuffix.TrimEnd('/')

$ip = Get-AzureRmPublicIpAddress -Name $ipName -ResourceGroupName $resourceGroupName
$ipAddress = $ip.IpAddress
$rUserName = "admin"
 
$jobcfg = "$path\$Global:PackageName\app_data\jobs\continuous\DataPrepare\DataPrepare.exe.config"
$webcfg =  "$path\$Global:PackageName\Web.Config"

$jobtpl = $jobcfg +".tpl"
$webTpl = $webcfg +".tpl"

if(!(Test-Path $jobtpl))
{
    Copy-Item $jobcfg $jobtpl
}

if(!(Test-Path $webTpl))
{
    Copy-Item $webcfg $webTpl
}

(Get-Content $jobtpl).Replace('[[StorageConnection]]', $accoutConnection).Replace("[[DatabaseConnection]]", $dbConnection).Replace("[[IPAddress]]", $ipAddress).Replace("[[RServerUser]]", $rUserName).Replace("[[Password]]", $VMPassword) | Set-Content $jobcfg
(Get-Content $webTpl).Replace('[[StorageConnection]]', $accoutConnection).Replace("[[DatabaseConnection]]", $dbConnection) | Set-Content $webcfg

$webApp = Get-AzureRmWebApp -ResourceGroupName $resourceGroupName -Name $siteName
$profilePath = $Env:TEMP + "\" + $siteName + "_src.json"
$profile = Get-AzureRmWebAppPublishingProfile -WebApp $webApp -OutputFile $profilePath -ErrorAction Stop
Write-Host "Published Profile Downloaded" -ForegroundColor Green

[FTpInfo]$uploadInfo = ParseFtp -profile $profile
Write-Host "FTP Connection Successfully Parsed" -ForegroundColor Green
 
$contentPath = "$path\$Global:PackageName"
Upload-Directory -ftp $uploadInfo -path $contentPath -ErrorAction Stop
Write-Host "Files successfully uploaded to FTP" -ForegroundColor Green