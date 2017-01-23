Write-Verbose "Running settings script at $PSCommandPath"

# Package name
$PackageName = 'CRDAnalytics'

# Deployment location: 'China North' or 'China East'
$Location = 'China North';

# Name prefix for resource group name, service plan name, storage account name, etc.
$Prefix = 'CRDA'

# Name suffix for resource group name, service plan name, storage account name, etc.
$Suffix = ([System.Environment]::TickCount -band [System.Int32]::MaxValue).ToString("D")

# Resource group name
$ResourceGroupName = "$($Prefix)RG$($Suffix)"

# Service plan name
$ServicePlanName = "$($Prefix)SP$($Suffix)"

# Web app name
$WebAppName = "$($Prefix)WA$($Suffix)"

# Database server name
$DatabaseServerName = "$($Prefix)DB$($Suffix)"

# Database name
$DatabaseName = "$($Prefix)$($PackageName)"

# Database user name
$DatabaseUserName = "CRDAUser"

# Database user password
$DatabasePassword = "P@ssw0rd!@#"

# PowerShell scripts path
$PsScriptsPath = Split-Path -Parent $PSCommandPath

# Package root path
$PackageRootPath = Split-Path -Parent $PsScriptsPath

# SQL scripts path
$SqlScriptsPath = "$PackageRootPath/SqlScripts"

# Initial database SQL script path
$InitialDatabaseSqlScriptPath = "$SqlScriptsPath/InitialDatabase.sql"

# Insert Sample Data SQL script path
$InsertSampleDataSqlScriptPath = "$SqlScriptsPath/InsertSampleData.sql"

# App service host path
$AppServiceHostPath = "$PackageRootPath/AppServiceHost"

# App service host web.config path
$AppServiceHostWebConfigPath = "$AppServiceHostPath/web.config"

# Web App profile path
$WebAppProfilePath = "$PackageRootPath/$WebAppName.profile.xml"

# Connection String path
$ConnectionStringPath = "$PackageRootPath/$DatabaseServerName.$DatabaseName.txt"