#load "..\CiqsHelpers\All.csx"
#r "System.IO.Compression" 

using System;
using System.IO;
using System.IO.Compression;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("configpipeline function begin");
    try
    {
        var parametersReader = await CiqsInputParametersReader.FromHttpRequestMessage(req);
    
        string sqlServerName = parametersReader.GetParameter<string>("sqlServerName"); 
        string sqlDbName = parametersReader.GetParameter<string>("sqlDatabaseName"); 
        string sqlServerUsername = parametersReader.GetParameter<string>("sqlServerUsername"); 
        string sqlServerPassword = parametersReader.GetParameter<string>("sqlServerPassword"); 

        string emotionAppUrl = parametersReader.GetParameter<string>("emotionAppUrl"); 
        string topicAppUrl = parametersReader.GetParameter<string>("topicAppUrl"); 
        string wordsAppUrl = parametersReader.GetParameter<string>("wordsAppUrl"); 

        string storageAccountName = parametersReader.GetParameter<string>("storageAccountName");
        string storageAccountKey = parametersReader.GetParameter<string>("storageAccountKey");
        string containerName = parametersReader.GetParameter<string>("containerName");

        var dbConnString = $"jdbc:sqlserver://{sqlServerName}.database.windows.net:1433;databaseName={sqlDbName};user={sqlServerUsername};password={sqlServerPassword}";

        var configFilePath = @"d:\home\site\wwwroot\configpipeline\Pipeline\config.xml";
        var configTemplate = File.ReadAllText(configFilePath);
        var configText = configTemplate.Replace("{{databaseConnection}}", dbConnString)
                                       .Replace("{{wordsURL}}", wordsAppUrl)
                                       .Replace("{{emotionURL}}", emotionAppUrl)
                                       .Replace("{{topicURL}}", topicAppUrl);

        File.WriteAllText(configFilePath, configText);

        var zipFilePath = @"d:\home\site\wwwroot\configpipeline\Pipeline.zip";
        using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.OpenOrCreate))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                foreach (var filePath in Directory.GetFiles(@"d:\home\site\wwwroot\configpipeline\Pipeline"))
                {
                    ZipArchiveEntry readmeEntry = archive.CreateEntry("webapps/" + Path.GetFileName(filePath));
                    File.OpenRead(filePath).CopyTo(readmeEntry.Open());
                }
            }
        }

        var storageCredentials = new StorageCredentials(storageAccountName, storageAccountKey);
        var storageAccount = new CloudStorageAccount(storageCredentials, true);
        var storageClient = storageAccount.CreateCloudBlobClient();

        var container = storageClient.GetContainerReference(containerName);
        container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

        CloudBlockBlob blob = container.GetBlockBlobReference(Path.GetFileName(zipFilePath));
        blob.UploadFromStream(File.OpenRead(zipFilePath));
    }
    catch (Exception ex)
    {
        log.Info(ex.GetType().Name);
        log.Info(ex.Message);
        log.Info(ex.StackTrace);
    }
    
    log.Info("configpipeline function end");
    
    return new object();
}