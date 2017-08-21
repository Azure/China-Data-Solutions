# 精准营销解决方案

## 场景描述

## 技术架构
解决方案架构图如下：
![Solution Diagram](./Pictures/arch.JPG)

解决方案展示了利用SQL Server 2016 和 R Services在通过对历史数据的分析，根据不同的潜在客户给出不同的行动方案，从而来提高一个营销活动中目标潜在客户的购买率。 
通过使用powerBI 做一个直观的展示。 

## PowerBI 演示
通过powerBI不仅可以直观的看到当前活动的建议，还可以看到用于训练模型的历史数据。
campaingn Summary 表展示的是历史数据，这些数据用于训练机器模型来预测活动的转化率。
Recommendation 表显示的是针对下一次的活动，机器模型给出的建议。
## 部署包
解决方案提供给可以部署在自有Azured订阅的解决方案部署包，用户可以一键部署本解决方案，从而深入了解如何利用Azure服务来实现业务场景。

### 部署前提
进行精准营销的解决方案的时，已默认具有以下的条件：
    1. 具有Azure中国的订阅账号
    2. 订阅账号中有以下的资源配额和权限
         i. 能够创建存储账号
        ii. 能够创建SQL Server 服务器
    3. 系统安装Azure SDK 和PowerShell 5.0
    4. 具有powerBI的订阅用于制作报表
### 部署
    1. 打开PowerShell，切换当前文件夹到DeployPkg下
    2. 运行deploy.ps1
    3. 在弹出的登录框中输入Azure订阅的用户名和密码，验证登录。
    4. 根据提示输入相关的信息，注意
        a. 输入数据库服务器的用户名azure会有安全性的限制，比如azure，sa不能作为该用户名。
        b. 数据库服务器密码，需要最短8位，包含大小写字母，数字及特殊字符，如Passw0rd!。
    5. 等待程序运行结束
部署完成之后，可以登录到Azure portal中查看相应的资源信息，可以通过连接对应的数据库来查看相关的数据。

## 源代码
源代码相关请参考: https://github.com/Microsoft/r-server-campaign-optimization/tree/master/SQLR