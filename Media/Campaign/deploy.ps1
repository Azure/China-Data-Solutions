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
 [Parameter(Mandatory=$True)]
 [string]
 $subscriptionId,

 [Parameter(Mandatory=$True)]
 [string]
 $resourceGroupName,

 [string]
 $resourceGroupLocation="China East",

 [Parameter(Mandatory=$True)]
 [string]
 $deploymentName,

 [Parameter(Mandatory=$True)]
 [string]
 $VirtualMachineName,

# [Parameter(Mandatory=$True)]
 [string]
 $VMUser="azure",

 [Parameter(Mandatory=$True)]
 [string]
 $VMPassword,

 [Parameter(Mandatory=$True)]
 [string]
 $SQLuser,

 [Parameter(Mandatory=$True)]
 [string]
 $SQLPassword,

 <#
 [string]
 $WorkspaceCollectionName = "CampDemoPBIC",

[string]
 $WorkspaceName = "CampDemoPBIW",

[string]
 $PBIXdatasetname = "CampDemodataset",

[string]
 $PBIApiEndpoint = "https://api.powerbi.cn",

[string]
 $PBIXfilepath = "Camp.pbix",

 [string]
 $webAppName  = "SNADemoPbiEmbed",

 [string]
 $servicePlanName ="CampDemo",

$Global:PackageName = "SNADemo",

$Global:PBIPackageName = "PowerBIEmbeded",
#>


 [string]
 $templateFilePath = "template.json",

 [string]
 $parametersFilePath = "parameters.json"
)

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
Login-AzureRmAccount -EnvironmentName AzureChinaCloud;

# select subscription
Write-Host "Selecting subscription '$subscriptionId'";
Select-AzureRmSubscription -SubscriptionID $subscriptionId;



# Register RPs
$resourceProviders = @("microsoft.compute","microsoft.network","microsoft.storage");
if($resourceProviders.length) {
    Write-Host "Registering resource providers"
    foreach($resourceProvider in $resourceProviders) {
        RegisterRP($resourceProvider);
    }
}

#Create or check for existing resource group
$resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
if(!$resourceGroup)
{
    Write-Host "Resource group '$resourceGroupName' does not exist. To create a new resource group, please enter a location.";
    if(!$resourceGroupLocation) {
        $resourceGroupLocation = Read-Host "resourceGroupLocation";
    }
    Write-Host "Creating resource group '$resourceGroupName' in location '$resourceGroupLocation'";
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation
}
else{
    Write-Host "Using existing resource group '$resourceGroupName'";
}

#there is something wrong, the tickcount return a -1986045140,result in can not create storage account
#$postfix = [System.Environment]::TickCount.ToString()
$postfix = (0..9|Get-Random -count 10) -join $null
$VMName = "$VirtualMachineName$postfix".ToLower()

if($VMName.Length -gt 15)
{
    $VMName = $VMName.Substring(0,15).ToLower()
    Write-Host $VMName
}

$newParametersPath = ".\newparameters.json"
Write-Host "begin to generate real parameters file"

(Get-Content -Path $parametersFilePath) -replace("{{envname}}", $VMName.ToLower()) -replace("{{VMUser}}", $VMUser.ToLower()) -replace("{{password}}", $VMPassword) -replace("{{subscriptionId}}",$subscriptionId) -replace("{{sqluser}}",$SQLuser) -replace("{{sqlpassword}}",$SQLPassword)| Set-Content $newParametersPath

# Start the deployment
Write-Host "Starting deployment..."

if(Test-Path $newParametersPath) {
    New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath -TemplateParameterFile $newParametersPath
} else {
    New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath
}

#download demo script
Write-Host "start download demo scrpit"

$files = @("https://yahuistorage2.blob.core.chinacloudapi.cn/campaign/r-server-campaign-optimization.zip","https://yahuistorage2.blob.core.chinacloudapi.cn/campaign/extension.ps1")
Set-AzureRmVMCustomScriptExtension -ResourceGroupName $resourceGroupName -Location $resourceGroupLocation -VMName $VMName -Name "extensionscript" -FileUri $files -Run "extension.ps1" -Argument "-ServerName $VMName -DBName campaign -username $SQLuser -password $SQLPassword"
Write-Host "install package success"
exit
