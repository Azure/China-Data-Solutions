----------

# 解决方案部署文档
----------


## 文件结构
当前所在的目录文件结构如下：

- DeployPkg包含部署所需要的文件
> - 项目文件包：socialSentiment.zip

- src包含所有源文件

> - 解决方案提供了相应的源代码包供用户参考，用户可以通过修改源码来快速搭建及开发基于Azure的定制化的Demo。


## 资源要求
下面是创建这个解决方案所需要的账户和软件：

1. DeployPkg文件夹包含的文件


1.  网络连接


1.  一个微软Azure的账户（https://portal.azure.cn）


1.  安装Windows PowerShell ISE


## 部署步骤
部署过程中，部署脚本会在Azure上自动创建位于中国北部数据中心的资源组。数据库包被上传到资源组下的存储账户里，数据库服务器被自动创建并导入所需数据库。数据库导入完毕后，资源文件被自动上传并建立Web应用。部署脚本执行完毕后PowerShell ISE 会返回发布成功的网站的地址。下面是部署的步骤：

1.  解压项目文件包到当前目录


1. 打开Windows PowerShell ISE


1. 输入命令，切换到部署文件所在位置 


1. 输入命令执行脚本setup.ps1
	-	首先页面会弹出窗口提示登陆azure，输入用户名和密码后点确定，脚本将自动继续运行。脚本运行过程中会有提问相关问题。
	-	问题一“Please select the deployment location [1] China North [2] China East”，请选择“1”或“2”后“回车”。
	-	问题二“Please Input a database connection string if you have. Press enter to create a new database.” ，请输入”回车”，就会创建新的数据库。
	-	问题三“Please input the password for sql database”，请输入密码Passw0rd! (可以自定义)，回车，继续执行脚本文件，创建新的Database Server后会自动导入数据库。

脚本执行完后最下方一行会显示已经发布好的网站的地址，进入这个地址就会看到部署的方案。

部署完毕后，登录 https://portal.azure.cn ，在Azure上会看到自动创建的以‘RGsocialsenti'为开头的资源组，在此资源组里可以看到本方案用到的所有资源：一个数据库服务器，一个数据库，一个存储账户，一个应用服务计划和一个应用服务。

## 重新部署
重新部署的主要操作是删除这个方案所在的资源组。登录 https://portal.azure.cn 在“资源组”菜单下找到以‘RGsocialsenti'为开头的资源组。点击这个资源组，可以看到右侧显示区域列出该资源组下的相关资源，点击资源组右侧的“删除” 即可删除整个资源组。

