# 解决方案部署文档

## 资源要求

1.安装 Windows PowerShell ISE

2.在 https://portal.azure.cn 创建Azure账户



## 架构描述

- 数据库文件：socialSentimentdb.zip
- 项目文件包：socialSentiment.zip
- 部署脚本：setup.ps1

> **注:**

> - setup.ps1 文件要和数据库文件放在同一文件目录下
> - setup.ps1 文件要和项目文件包放在同一文件目录下
> - setup.ps1 文件的第二行 PackageName 代表项目文件的名称
> - setup.ps1 文件的第四行 PackageName 代表项目使用的数据库的名称




## 部署步骤

使用文件中的数据库创建一个数据库作为这个网站的数据库。脚本执行过程中，会创建新的数据库，创建一个资源组，上传“项目发布文件”。

- 解压项目文件包到当前目录
- 打开 Windows PowerShell ISE
- 使用命令 cd ，切换到部署文件所在位置 
- 输入 setup.ps1 按下键盘上 tab 键两下，然后回车，脚本文件即开始执行。
	-	首先页面会弹出窗口提示登陆 azure ，输入用户名和密码后点确定，脚本将自动继续运行。运行脚本过程中会有相关问题
	-	第一个：Please select the deployment location [1] China North [2] China East 。请输入“1” 并“回车”。
	-	第二个：Please Input a database connection string if you have. Press enter to create a new database. 
请输入”回车”。
	-	第三个：Please input the user name for sql database, sa is not allowed 。请输入azure（这里可以自定义）
	-	第四个：Please input the password for sql database。请输入密码 Passw0rd! (这里可以自定义)，回车，继续执行脚本文件。创建新的Database Server后会自动导入数据库。
	-   数据库导入完毕后，资源文件被自动上传到FTP。
- 脚本执行完毕后，PowerShell ISE 会返回发布成功的网站的地址。

