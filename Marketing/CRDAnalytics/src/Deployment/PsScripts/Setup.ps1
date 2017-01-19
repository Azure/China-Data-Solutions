###############################################################################
# Prepare Script Execution Environment
###############################################################################

[CmdletBinding(SupportsShouldProcess=$true)]
Param()

Function Show-Message
{
	param
	(
	   [parameter(Mandatory=$true)]
	   [string]
	   $message,
	   [parameter(Mandatory=$false)]
	   [string]
	   $color = 'Gray'
	)

	Write-Host "$([System.DateTime]::Now.ToString("o")) $message" -ForegroundColor $color
}

Show-Message -message "Prepare script execution environment begin."

Write-Verbose "Running environment setup script at $PSCommandPath"

Write-Verbose "Loading Functions.ps1 script..."
. $PSScriptRoot/Functions.ps1

Write-Verbose "Loading Setup.ps1 script..."
. $PSScriptRoot/Settings.ps1

Check-AzureRmModule

Check-Net45

Show-Message -message "Prepare script execution environment end."

###############################################################################
# Setup Azure Mooncake Environment
###############################################################################

Show-Message -message "Setup Azure Mooncake environment begin."

$mcprofile = Add-AzureRmAccount -EnvironmentName AzureChinaCloud -ErrorAction SilentlyContinue

if (!$mcprofile) {
	Write-Error "Can't get Azure Mooncake profile"
	Break
}

# Check resource group, will create if not exist
Check-ResourceGroup -resourceGroupName $ResourceGroupName -resourceGroupLocation $Location

# Create SQL Azure database and get connection string, will create if not exist
$connectionString = Create-SqlAzureDatabase -resourceGroupName $ResourceGroupName -databaseServerName $DatabaseServerName.ToLower() -location $Location -databaseName $DatabaseName -databaseUserName $DatabaseUserName -databasePassword $DatabasePassword

# Create database schema
Exec-SqlScript -connectionString $connectionString -scriptPath $InitialDatabaseSqlScriptPath

# Insert sample data
Exec-SqlScript -connectionString $connectionString -scriptPath $InsertSampleDataSqlScriptPath

# Set database connection string in web.config file for App Service host
(Get-Content $AppServiceHostWebConfigPath).Replace("{{DatabaseConnectionString}}", $connectionString) | Set-Content $AppServiceHostWebConfigPath

# Check service plan, will create if not exist
Check-ServicePlan -resourceGroupName $ResourceGroupName -servicePlanName $ServicePlanName -location $Location

# Create azure web app
$webApp = Create-AzureWebApp -resourceGroupName $ResourceGroupName -webAppName $WebAppName -servicePlanName $ServicePlanName -location $Location -profilePath $WebAppProfilePath

# Enable always on feature for azure web app
Set-AzureRmResource -PropertyObject @{"siteConfig" = @{"AlwaysOn" = $true}} -Name $WebAppName -ResourceGroupName $ResourceGroupName -ResourceType Microsoft.Web/sites -Force

# Start azure web app
$result = Invoke-WebRequest -Uri "http://$($webApp.DefaultHostName)/" -ErrorAction SilentlyContinue

Show-Message -message "Setup Azure Mooncake environment end."
