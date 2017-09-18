$path = Split-Path -Parent $MyInvocation.MyCommand.Definition
Expand-Archive -Path $path\r-server-campaign-optimization.zip -Destination c:\
cd c:\r-server-campaign-optimization\SQLR
powershell -ExecutionPolicy Unrestricted -file c:\r-server-campaign-optimization\SQLR\Campaign_Optimization.ps1 $args
#Write-Host "$args"
#powershell -ExecutionPolicy Unrestricted -file e:\Campaign_Optimization.ps1 $args
#Unzip-File -ZipFile r-server-campaign-optimization.zip -TargetFolder f:\test
Write-Host "successed"