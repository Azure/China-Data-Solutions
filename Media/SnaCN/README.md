# 传播主体分析解决方案
 
xxxxxxxxxxxxxxxxxx

## 场景描述
 
xxxxxxxxxxxxxxxxxxxxxxxxx

## 技术架构
解决方案架构图如下：

![Solution Diagram](./Pictures/.JPG)

解决方案是一个从数据收集到数据展现的完整流程，其中包括了数据收集，数据注入，数据转换，数据存储和分析，以及数据可视化部分。

数据收集部分是一个WebJob，主要用于将示例数据注入到事件中心，示例数据可以根据需要进行更改或者自行生成。
数据存储主要用于存储业务数据，存放在SQLAzure 数据库中。
分析工作主要使用WebJob作为后台服务运行进行数据处理操作，WebJob运行分析作业将数据从存储中加载出来，将分析处理完成的数据存储到数据库中。
数据可视化则通过将Power BI连接到SQL Azure数据源上，根据需要的数据进行报表构建，然后把Power BI文件嵌入网站页面的方式实现，。


## [部署包](./DeployPkg)
解决方案提供了可部署在自有Azure订阅的解决方案部署包，通过部署解决方案，用户可以深入了解如何利用Azure云服务实现业务场景，并可以通过修改部署包中的样本数据或修改PBI Demo报告来快速搭建自己的Demo。

### 部署前提
要进行舆情洞察解决方案的部署，必须具备如下的资源和条件
  1. 具有Azure中国的[订阅账号](https://www.azure.cn/)
  2. 订阅账号中包含足够可用的资源配额和权限
     1. 能够创建存储账号（Storage Account）
     2. 能够创建虚拟机
     3. 能够创建SQL Azure服务器和数据库以及相关防火墙规则
     4. 能够创建服务计划（Service Plan）和网站应用（Web App）
     5. 能够创建Power BI工作区集合
  3. 系统安装Azure SDK及Powershell 5.0
  4. 具有Power BI订阅用于制作展示报表
  5. 安装Node.js
  6. 安装PowerBI-cli

### [部署包组成]((./DeployPkg))
部署包中主要包含以下几个部分
  1. deploy.ps1 用于进行自动化部署的主要脚本。
  2. SNADemo文件夹中是对应用于。
  3. PowerBIEmbeded文件夹
  4. Initdb.sql用于初始化数据库表及索引结构
  5. 需要将源代码中的SNASite发布到该文件夹中的SNADemo文件夹下，详细参见[操作步骤](####发布xxx)
  5. 需要将源代码中的PowerBIEmbeded发布到该文件夹中的PowerBIEmbeded文件夹下，详细参见[操作步骤](####发布Web应用用于PowerBI Embeded展示)

### 发布

#### 发布xxx

要构建完成的部署包，需要将源代码进行生成发布，作为部署包的部署内容。

  1. 源代码文件夹SNASite，打开源代码工程，选择SNASite工程，点击右键，选择**发布(Publish)**.
  2. 在弹出的页面中, 选择**自定义（Custom）**。点击下一步。
  3. 在接下来的**发布方式**（Publish Method）中选择 **文件系统**。
  4. 打开[部署包](./DeployPkg)文件夹，在该文件夹中新建文件夹**SNADemo**。
  5. 在**目标位置**中选取新建的文件夹SNASite作为目标位置
  6. 点击下一步，选择**发布**版本。
  7. 点击**完成**，进行工程的生成和发布。发布完成之后可以在SNADemo文件夹中看到完整的网站应用结构文件。

#### 发布Web应用用于PowerBI Embeded展示

要构建完成的部署包，需要将源代码进行生成发布，作为部署包的部署内容。

  1. 源代码文件夹PowerBIEmbeded，打开源代码工程，选择PbiEmbedWeb工程，点击右键，选择**发布(Publish)**.
  2. 在弹出的页面中, 选择**自定义（Custom）**。点击下一步。
  3. 在接下来的**发布方式**（Publish Method）中选择 **文件系统**。
  4. 打开[部署包](./DeployPkg)文件夹，在该文件夹中新建文件夹**PowerBIEmbeded**。
  5. 在**目标位置**中选取新建的文件夹PowerBIEmbeded作为目标位置
  6. 点击下一步，选择**发布**版本。
  7. 点击**完成**，进行工程的生成和发布。发布完成之后可以在PowerBIEmbeded文件夹中看到完整的网站应用结构文件。


### 部署流程

部署流程包含了在Azure订阅中进行资源创建的过程。完成的创建过程包含了以下步骤。

  1. 创建资源组、虚拟机、数据库。
  2. 将Web应用文件使用FTP的方式上传到Web应用中。



### 部署步骤

  1. 打开PowerShell，切换当前文件夹到DeployPkg下面。
  2. 运行deploy.ps1。
  3. 根据提示，输入订阅ID
  4. 根据提示，输入资源组名称
  5. 根据提示，输入虚拟机名称
  6. 根据提示，输入虚拟机密码需要最短8位，包含大小写字母，数字及特殊字符，如Passw0rd!。
  7. 根据提示，输入deploymentName  
  8. 在弹出的登录框中输入Azure订阅的用户名和密码，验证登录。
  9. 根据提示，输入部署地点chinanorth。
  5. 数据库服务器的用户名，比如azure，sa不能作为该用户名。
  6. 输入数据库服务器密码，需要最短8位，包含大小写字母，数字及特殊字符，如Passw0rd!。
  7. 等待脚本运行，直到部署完成。

部署完成之后，可以登录到Azure Portal中，在资源组中通过SSCN进行筛选，找到对应的资源组进行资源相关信息查看和管理。




## [获取数据解决方案的源代码](./src/SNADemo)
解决方案提供了相应的源代码包供用户参考。用户可以通过修改源码来快速搭建及开发基于Azure的定制化的Demo。通过使用Visual Studio打开\src\SNASite\SNASite.sln文件可以打开工程。

源代码主要包含以下几个工程
  1. DataPrepare，这是一个WebJob工程，主要用于模拟数据的收集
  2. DataLibrary，主要包含了文本处理的相关逻辑。
  3. SNASite,用于发布到Azure网站应用的工程，WebJob将随该工程进行发布并运行。系统的部署也需要先将该工程进行发布，形成整体的部署包。

## [获取数据解决方案的源代码](./src/PowerBIEmbeded)
解决方案提供了相应的源代码包供用户参考。用户可以通过修改源码来快速搭建及开发基于Azure的定制化的Demo。通过使用Visual Studio打开\src\PowerBIEmbeded\CreatePbiEmbedded.sln文件可以打开工程。

源代码主要包含以下几个工程
  1. PbiEmbedWeb,用于发布到Azure网站应用的工程。系统的部署也需要先将该工程进行发布，形成整体的部署包。

