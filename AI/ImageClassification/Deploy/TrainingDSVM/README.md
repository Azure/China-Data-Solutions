# 如何部署 TrainingDSVM [English](README-EN.md)

本解决方案包括两个流程：
1. 训练流程
2. 部署流程

**TrainingDSVM** 对应于训练流程。按照文档部署后，会在你的Azure订阅中自动创建出一个数据科学虚拟机(DSVM)。所有训练需要用到的依工具和依赖包都会自动安装好，代码和样本数据也都会部署在数据科学虚拟机中。在部署完成之后，用户就可以直接在 Jupyter Notebook 中探索代码和数据。

## 如何部署

### 部署前提
要进行本解决方案的部署，必须具备如下的资源和条件：
1. Azure中国的[订阅账号](https://www.azure.cn/)
2. Azure Powershell SDK 及 Powershell. 如果没有安装Powershell, 可以从[这里](https://github.com/Azure/azure-powershell/releases) 下载安装。
3. 本方案自动部署的是 Windows 版本的数据科学虚拟机(DSVM)。 如果用户想要使用Linux版本的数据科学虚拟机(DSVM)，可以参考[手动部署](#可选-手动部署).

### 部署步骤
1. 打开Powershell, 进入当前目录下（ *TrainingDSVM* ）。
2. 运行以下命令 
   ``` powershell
   .\deploy.ps1
   ```
3. 按照提示输入订阅Id（GUID）, 资源组名字，虚拟机名字，虚拟机用户名，虚拟机密码，部署名字等。
4. 当登录弹框出来的时候，输入Azure中国的用户名 ( \*@\*.cec.partner.onmschina.cn ) 密码登录。
5. 等待脚本运行，直到部署完成。 

部署成功后，用户就可以在[Azure门户](https://portal.azure.cn/)上看到这个虚拟机。点击*连接* 就可以远程登录到这台数据科学虚拟机(DSVM)中了。

## 运行示例代码
第一次打开 Jupyter Notebook, 先设置密码。只需要双击桌面的快捷方式 *Jupyter SetPassword* 按照提示设置密码 (密码可以设置为空)。设置完成后，双击桌面上的快捷方式 *Jupyter Start* 启动 Jupyter 服务。最后可以直接双击桌面快捷方式 *Jupyter Notebook* 或者打开浏览器导航到 https://localhost:9999/ 。输入刚才设置的密码登录成功后，可以看到一系列的 Notebooks，都是 DSVM 里面提供的示例代码。其中本方案的示例代码和数据在文件夹 *image classification* 下面。

打开 *ImageClassification.ipynb* 文件时，可能需要你选择内核 kernel, 选择 [conda env:py35] .


## (可选) 手动部署
本方案自动部署的是 Windows 版本的 DSVM。 如果用户想要使用 Linux 版本的 DSVM, 或者就是想尝试手工部署，可以参考以下步骤：
1. 创建一个DSVM。可以使用任何方式，比如 Powershell, Azure Cli 或者 Azure 门户。
2. 在DSVM虚拟机 *c:\dsvm* 目录下，创建一个新的文件夹叫 *imageclassification* 。*c:\dsvm\notebooks\* 是Jupyter Notebook的根目录。拷贝本仓库的 *Code* 文件夹里面的所有内容到 DSVM 的 *c:\dsvm\notebooks\imageclassification* 目录下。再把本仓库 *Data* 文件夹下面的 *image*文件夹整个拷贝到虚拟机的 *c:\dsvm\notebooks\imageclassification* 下面。
3. 安装需要的 python 包，本方案需要安装以下这些包：
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
ggplot
```
如果你不会安装 python 包，可以参考以下方法：把当前目录下的 *requirements.txt* 拷贝到 DSVM 的 *c:\\* ，在 DSVM 中打开 Anaconda Prompt (py35)，在 Anaconda 中运行
```
cd c:\
pip install -r requirements.txt -i https://mirror.azure.cn/pypi/simple
```

4.启动 Jupter Notebook。 示例程序在目录 *imageclassification* 下。如果你第一使用Jupyter Notebook, 你需要设置密码。具体参考[运行示例代码](#运行示例代码)
