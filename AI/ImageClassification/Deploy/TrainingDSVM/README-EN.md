# How to deploy TrainingDSVM [中文](README.md)
As our Architecure shows as following, there are two pipelines:
1. Training Pipeline
2. Inference Pipeline

TrainingDSVM is focus on traning pipline.
Using these deploy scripts, you can create an Data Science Virtual Machine ( DSVM ) in your subscription. 
All dependencs will installed automaticly. Code will be copied into this DSVM as well. You can explore sample code and data in Jupyter Notebook just after deployment.

## How to deploy

### Prerequisite
You should have following prerequisites:
1. An subscription of [China Azure](https://www.azure.cn/) 
2. Powershell and Azure PowerShell SDK. if not, download from [this link](https://github.com/Azure/azure-powershell/releases) and installed.  
3. We deploy windows version DSVM in this script, so following explaination is on windows version. If you prefer Linux version, please refer the latest part: [How to deploy manually](#optional-how-to-deploy-manually).

### Deploy steps
1. Open Powershell, navigate to current folder (*TrainingDSVM*).
2. Run Commond 
   ``` powershell
   .\deploy.ps1
   ```
3. Provide subscriptionId, resourceGroupName,VMName,VMUserName,VMUserPassword,deploymentName as requirement.
4. Log in your Azure china account (like \*@\*.cec.partner.onmschina.cn) when login dialog prompted.
5. Wait a few minutes until the scripts runing sucessfully.


After deployed sucessfully, you can see this VM in your [Azure Portal](https://portal.azure.cn/). Click connect and using remote desktop to connect to this VM.

## How to explore demo code
If you use this VM first time, you should set a Jupyter Notebook password before start Jupyter Notebook. Just  click the shortcut *Jupyter SetPassword* on desktop and following instruction. (You can set the password to empty)
Then click the shortcut *Jupyter Start* to start jupyter server. Then open Jupyter Notebook by click shortcut *Jupyter Notebook* or just open an explorer and navigate to https://localhost:9999/ .
After login, you can see a list of build in notebooks, while this demo code and data will under the folder *ImageClassification*. When you open *ImageClassification.ipynb*, it may pop a dialog to choose a kernel. Choose "[conda env:py35]" and set kernel.


## (Optional) How to deploy manually
This deploy package automaticly creates DSVM with Windows version. If you prefer to use Linux version or you just want to know more about deployment, following manual steps as following:
1. Create a DSVM using any way you like, Powershell, Azure cli or just on Portal.
2. Create a new folder named *imageclassification* under *c:\dsvm\notebooks\* in your DSVM. *c:\dsvm\notebooks\* is the root folder of Jupyter Notebook in DSVM. Copy all contents under *Code* folder in this repo to *c:\dsvm\notebooks\imageclassification* folder in this DSVM. Copy *image* folder under *Data* to the *c:\dsvm\notebooks\imageclassification* folder in your DSVM. 
3. Install necessary python packages, in this demo case, including following package:
```
numpy
scipy
opencv-python
imutils
keras
scikit_image
scikit-learn
h5py
azure.storage.blob
```
If you don't know about to install python packages. You can copy *requirements.txt* in this folder to *c:\\* of DSVM, open Anaconda Prompt (py35), in Anaconda console, run following:
``` 
cd c:\
pip install -r requirements.txt -i https://mirror.azure.cn/pypi/simple
```

4. Start Jupyter Notebook. And this demo's code and data will on the folder *imageclassification*. Before start Jupyter Notebook, you need set the password and start Jupyter Server. Please refer preview setction [How to explore demo code](#how-to-explore-demo-code)
