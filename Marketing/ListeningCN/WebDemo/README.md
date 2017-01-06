# 解决方案部署文档

## 资源要求

1.安装Windows PowerShell ISE

2.在https://portal.azure.cn 创建Azure账户



## 架构描述

- 数据库文件：socialSentimentdb.zip
- 项目文件包：socialSentiment.zip

> **注:**

> - setup.ps1文件要和数据库文件放在同一文件目录下
> - setup.ps1文件要和项目文件包放在同一文件目录下
> - setup.ps1文件的第二行 PackageName 代表项目文件的名称
> - setup.ps1文件的第四行 PackageName 代表项目使用的数据库的名称




## 部署步骤

使用文件中的数据库创建一个数据库作为这个网站的数据库。脚本执行过程中，会创建新的数据库，创建一个资源组，上传“项目发布文件”。也可以在脚本执行过程中，当提示问题“Please Input a database connection string if you have. Press enter to create a new database. ”时，输入已有的数据库连接字符串也可以。

- 解压项目文件包到当前目录
- 打开Windows PowerShell ISE
- 输入命令，切换到项目代码所在位置“绝对路径\PackageSocialSentiment\SocialSentiment” 
- 输入 setup.ps1 按下键盘上tab键两下，然后回车，脚本文件即开始执行。
	-	 首先页面会弹出窗口提示登陆azure，输入用户名和密码后点确定，脚本将自动继续运行。运行脚本过程中会有相关问题
	-	第一个：Please select the deployment location [1] China North [2] China East 。请输入“1” 并“回车”。
	-	第二个：Please Input a database connection string if you have. Press enter to create a new database. 
请输入”回车”。就会创建新的数据库
	-	第三个：Please input the user name for sql database, sa is not allowed 。请输入azure
	-	第四个：Please input the password for sql database。请输入密码Passw0rd! (这里区分大小写，倒数第四个字符是数字)，回车，继续执行脚本文件


