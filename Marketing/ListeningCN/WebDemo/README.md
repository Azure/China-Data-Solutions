# Social Listening Solution (CN)

## 摘要

## 前提

1.安装Windows PowerShell ISE

2.在https://portal.azure.cn 创建Azure账户



## 文件结构

> **注:**

> - setup.ps1文件的第四行代表项目使用的数据库的名称：
$Global:DbPackage = "socialmediadb3.zip"；




## 发布过程 

使用文件中的数据库cosialmediadb3.zip创建一个数据库作为这个网站的数据库。脚本执行过程中，会创建新的数据库，创建一个资源组，上传“项目发布文件”。也可以在脚本执行过程中，当提示问题“Please Input a database connection string if you have. Press enter to create a new database. ”时，输入已有的数据库连接字符串也可以。

- 打开Windows PowerShell ISE
- 输入命令，切换到项目代码所在位置“绝对路径\PackageSocialSentiment\SocialSentiment” 
- 输入命令执行脚本setup.ps1
	-	 首先页面会弹出窗口提示登陆azure，输入用户名和密码后点确定，脚本将自动继续运行。运行脚本过程中会有相关问题
	-	第一个：Please select the deployment location [1] China North [2] China East 。请输入“1” 并“回车”。
	-	第二个：Please Input a database connection string if you have. Press enter to create a new database. 
请输入”回车”。就会创建新的数据库
	-	第三个：Please input the user name for sql database, sa is not allowed 。请输入azure
	-	第四个：Please input the password for sql database。请输入密码Passw0rd! (这里区分大小写，倒数第四个字符是数字)，回车，继续执行脚本文件


