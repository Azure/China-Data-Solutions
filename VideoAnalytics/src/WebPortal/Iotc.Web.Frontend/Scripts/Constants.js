var registryUri = "{{RegistryUri}}";
var registryUserName = "{{UserName}}";
var registryPassword = "{{Password}}";

var EdgeDeviceConfigTemplateWindows = '<div style="margin-top: 1.5rem;">Add Device Success! Please follow steps to set up on your edge devices.</div>'
    + '<div style = "margin-top:1.5rem;"> 1. Download and install Docker on your edge devices.<br>'
    + '<a style="margin-left:2rem;" href="https://download.docker.com/win/stable/Docker%20for%20Windows%20Installer.exe">https://download.docker.com/win/stable/Docker%20for%20Windows%20Installer.exe</a>'
    + '</div>'
    + '<div style="margin-top:1.5rem;">'
    + '2. Download and install Python 2.7 on your edge devices.<br>'
    + '<a style="margin-left:2rem;" href="https://www.python.org/ftp/python/2.7.14/python-2.7.14.msi">https://www.python.org/ftp/python/2.7.14/python-2.7.14.msi</a>'
    + '</div>'
    + '<div style="margin-top:1.5rem;">3. Run following on Conmmands:'
    + '<div style="margin-left:2rem;">'
    + 'pip install -U azure-iot-edge-runtime-ctl <br><br>'
    + 'iotedgectl setup --connection-string "{device_connection_string}" --nopass <br><br>'
    + 'iotedgectl login --address ' + registryUri + ' --username ' + registryUserName + ' --password ' + registryPassword + ' < br > <br>'
    + 'iotedgectl start <br><br>'
    + '</div ></div>';



var EdgeDeviceConfigTemplateLinux = '<div style="margin-top: 1.5rem;">Add Device Success! Please follow steps to set up on your edge devices.</div>'
    + '<div style = "margin-top:1.5rem;"> 1. Install Docker CE on your edge devices and make sure that it\'s running.<br>'
    + '<a style="margin-left:2rem;" href="https://docs.docker.com/install/linux/docker-ce/ubuntu/#install-docker-ce">Install Docker for Linux</a>'
    + '</div>'
    + '<div style="margin-top:1.5rem;">'
    + '2. Install Python and pip on your edge devices.<br>'
    + '<span style="margin-left:2rem;">sudo apt-get install python-pip</span>'
    + '</div>'
    + '<div style="margin-top:1.5rem;">3. Run following Commands:'
    + '<div style="margin-left:2rem;">'
    + 'sudo pip install -U azure-iot-edge-runtime-ctl <br><br>'
    + 'sudo iotedgectl setup --connection-string "{device_connection_string}" --nopass <br><br>'
    + 'sudo iotedgectl login --address ' + registryUri + ' --username ' + registryUserName + ' --password ' + registryPassword + ' < br > <br>'
    + 'sudo iotedgectl start <br><br>'
    + '</div ></div>';