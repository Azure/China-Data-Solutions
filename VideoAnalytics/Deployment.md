# Deployment For Video Analytics on Azure IoT Edge

This instruction are used for deploy the video analytics on Azure IoT Edge,  after deployment, then you can monitoring one or multiple cameras for human detection and helmet alert. 

Requirement: 
1. An Azure Subscription which have Azure IoT Edge Supported. 
2. A device support docker runtime and can connect with internet. 
3. A RSTP video stream that can be accessed by the device. 

Deployment Steps. 

1. Set up an Azure IoT Hub following the [Instruction](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-create-through-portal "Instruction")

2. Set up a SQL database following the [Instruction](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-get-started-portal "Instruction")

3. Create a Storage Account following the [Instruction](https://docs.microsoft.com/en-us/azure/storage/common/storage-create-storage-account "Instruction")

4. [Then build and upload the docker image used for Azure IoT Edge](./docker build.md "Then build and upload the docker image used for Azure IoT Edge"). 

5. Connect with the SQL database and then Init the database tables, the sql script can be found in src/WebPortal/DataAccessLayer/SQL/init.sql 
6. Open the Iot.Web.sln in src/WebPortal, then replace the configuration section in the following location.
	a) Change the configuration in Web.Config in Iot.web.backend
	b) Change the configuration in App.Config in MessageHandler project
	c) For all those configuration change, you need replace *{{DatabaseConnection}} * with the sql data base connection string,  and Replace *{{StorageConnection}}* withe the storage account connection string, Other setting related with IoT Hub Connection String and Event Hub, you should find it with the IoT Hub Connection.  Wilth all those Configuration changed, then you are ready to deploy the project to Azure App Service. 

7.  Follow the follwing [instruction](https://docs.microsoft.com/en-us/azure/app-service/app-service-web-get-started-dotnet "instruction") to deploy IoT.Web.FrontEnd to Azure

8. After that, then connect to Azure Portal, and setup an extra application named *API* based on the following [instruction](https://blogs.msdn.microsoft.com/tomholl/2014/09/21/deploying-multiple-virtual-directories-to-a-single-azure-website/ "instruction")  and then publish IoT.Web.Backend to the newly created application. 

After all those deployment done, then you are ready to go for testing the video analytics platform with the following [guide](./Test.md "guide").