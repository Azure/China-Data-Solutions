Param(
    [parameter()]
    [string]
    $password="Passw0rd123!"
)

$folderName = "pmdeploy"
$zipfile = $folderName+".zip"
Invoke-WebRequest -Uri https://sharedstorage.blob.core.chinacloudapi.cn/deployr/DeployService.zip -OutFile $zipfile
Expand-Archive -Path $zipfile -Force
dotnet "C:\Program Files\Microsoft\R Server\R_SERVER\o16n\Microsoft.RServer.Utils.AdminUtil\Microsoft.RServer.Utils.AdminUtil.dll" -silentoneboxinstall $password
Start-Process -FilePath "C:\Program Files\Microsoft\R Server\R_SERVER\bin\Rscript.exe" -ArgumentList ".\pmdeploy\service.R" 
