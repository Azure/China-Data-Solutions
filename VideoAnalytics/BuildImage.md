# Build and Upload The Docker Image

To start the deployment for the video analytics solution,  you need firstly build the docker image which is the IoT Edge module. 

To build the docker image, you need firstly install [Docker for Windows](https://docs.docker.com/docker-for-windows/install/ "Docker for Windows"), and then install visual studio 2017 using for solution build (visual studio code is also recommended). As all the docker application are developed based on dotnet core, dotnet core 2.0 runtine or higher is required for build the docker image. Install dotnet core 2.0 SDK from [here](https://www.microsoft.com/net/download/windows). 

To finish the docker image building, following the next steps. 

1. Open the visual studio solution **linuxhatdemo.sln**
2. Build the solution and make sure the whole build pass. Or run **dotnet build** in command line under *linuxhatdemo* folder.
3. Right click on the project and click **publish**, then you should find all the binaries are published to the folder **bin\Release\PublishOutput**. Or Just run **dotnet publish** in command line under *linuxhatdemo* folder.
4. Then switch to the folder **linuxhatdemo**, and then run the docker build command **docker build --rm -f Docker\linux-x64\Dockerfile -t videoanalytics/linuxhatdemo:debug  bin\Release\PublishOutput**
5. When the image are built out, you can use **docker image list**  to check the docker images on your local.  You can see the image list and there will be a docker image with REPOSITORY **videoanalytics/linuxhatdemo** and TAG is  **debug**
6. To enable the docker image been used  by IoT Edge, you need set up the a Docker Container Registry on Azure, you can follow the [following steps](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-get-started-portal "following steps") to create and use a Azure docker container registry. 
7. After the docker container registry is created, you view the container registry and login with the following command 
**docker login --username {username} --password {password} {login server}**
8. Then you need tag the image with the loginserver. 
**docker tag videoanalytics/linuxhatdemo:debug {login server}/linuxhatdemo:debug**
9. Then you can use docker push to pushed the tagged image to the docker registry.
