----------

# 解决方案部署文档
----------


## 文件结构
当前所在的目录文件结构如下：

- DeployPkg包含部署所需要的文件
> - 数据库文件：socialSentimentdb.zip
> - 项目文件包：socialSentiment.zip
> - 部署脚本：setup.ps1

> **注:**
setup.ps1 文件要和数据库文件，项目文件包放在同一文件目录下；文件的第二行 PackageName 代表项目文件的名称；文件的第四行 PackageName 代表项目使用的数据库的名称。

- soc包含所有源文件

> - 解决方案提供了相应的源代码包供用户参考。用户可以通过修改源码来快速搭建及开发基于Azure的定制化的Demo。


## 资源要求
下面是创建这个解决方案所需要的账户和软件：

1. DeployPkg文件夹包含的文件
2. 网络连接
3. 一个微软Azure的账户（https://portal.azure.cn）
3. 安装Windows PowerShell ISE


##部署步骤
部署过程中，首先使用文件中的数据库创建一个数据库作为这个网站的数据库。然后创建一个资源组，上传“项目文件”，部署脚本执行完毕后PowerShell ISE 会返回发布成功的网站的地址。下面是部署的步骤：

- 解压项目文件包到当前目录
- 打开Windows PowerShell ISE
- 输入命令，切换到部署文件所在位置 
- 输入命令执行脚本setup.ps1
	-	首先页面会弹出窗口提示登陆azure，输入用户名和密码后点确定，脚本将自动继续运行。脚本运行过程中会有提问相关问题。
	-	问题一“Please select the deployment location [1] China North [2] China East”，请选择“1”或“2”后“回车”。
	-	问题二“Please Input a database connection string if you have. Press enter to create a new database.” ，
请输入”回车”，就会创建新的数据库。
	-	问题三“Please input the password for sql database”，请输入密码Passw0rd! (可以自定义)，回车，继续执行脚本文件，创建新的Database Server后会自动导入数据库。

脚本执行完后最下方一行会显示已经发布好的网站的地址，进入这个地址就会看到部署的方案。

