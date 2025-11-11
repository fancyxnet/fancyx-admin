# 风汐管理系统(fancyx-admin)

<!-- PROJECT SHIELDS -->

[![GitHub stars](https://img.shields.io/github/stars/fancyxnet/fancyx-admin?style=social)](https://github.com/fancyxnet/fancyx-admin)
[![GitHub forks](https://img.shields.io/github/forks/fancyxnet/fancyx-admin?style=social)](https://github.com/fancyxnet/fancyx-admin)
[![License](https://img.shields.io/badge/license-MIT-blue?style=flat-square)](https://github.com/fancyxnet/fancyx-admin)

## 项目介绍

风汐管理系统，适用微服务（Ocelot+Consul）和单体部署；项目使用.NET9+React18构建的RBAC通用权限管理系统（支持按钮级别权限），支持多租户功能，简单易上手，不使用任何三方Admin框架，完全由作者+AI独立开发；旨在为个人、企业提供高效、美观的后台管理解决方案，为.NET+React后台方案添砖加瓦， 系统采用最新最稳定的技术栈，具有良好的扩展性和可维护性，支持快速定制开发。

**核心特点**

* 支持多租户
* 按钮级别权限控制
* 部门级数据权限
* 简洁高效的用户界面
* 模块化的系统架构
* 可读性高代码结构
* 支持微服务和单体

**未来建设**

* 微服务和单体都支持（已实现）
* 持续修复发现BUG
* 持续优化前后端代码
* 增加ERP模块，检测微服务可行性和架构灵活性

## 代码仓库

* GitHub: https://github.com/fancyxnet/fancyx-admin
* Gitee: https://gitee.com/fancyxnet/fancyx-admin

## 在线预览

*可使用提供的多租户域名测试多租户功能*

在线文档： https://doc.crackerwork.cn <br/>
预览地址： https://crackerwork.cn <br/>
预览账号： admin <br/>
预览密码： 123qwe* <br/>

**如果在输入账号和密码后提示错误，请检查账号密码输入框中是否存在空格**

> 注意：预览是演示模式，对于核心数据是无法删除和编辑

## 使用技术

* .NET Core
* MySQL
* EFCore
* Aop
* Redis
* EventBus
* AutoMapper
* Autofac
* Castle.Core
* Serilog
* Consul
* Ocelot
* Orleans
* GRPC
* MQTT
* React
* Ant Design
* Vite
* Sass/SCSS

## 功能模块

* 组织架构
  * 职位分组
  * 职位管理
  * 部门管理
  * 通知管理
  * 我的通知
* 系统管理
  * 用户管理
  * 角色管理
  * 菜单管理
  * 数据字典
  * 配置管理
  * 租户管理
  * 登录日志
  * 业务日志
* 系统监控
  * 在线用户
  * 异常日志
  * 访问日志
* 快速开发
  * 富文本组件
  * 代码生成

## 系统截图

1. 登录
   ![登录](./docs/img/login.png "login")

2. 首页
   ![首页](./docs/img/home.png "home")

## 快速上手

### 项目结构

**后端**

| 库                        | 描述                                         |
| ------------------------ | ------------------------------------------ |
| Fancyx.Consul            | Consul服务注册与发现,配置中心                         |
| Fancyx.Core              | 最核心层，该层有服务自动注册、模块化加载、静态帮助类、当前用户/租户接口及中间件   |
| Fancyx.EfCore            | EFCore公共层，基础仓储类，工作单元等                      |
| Fancyx.EventBus          | 事件总线类，封装了DotNetCAP包，使用MySql确保正确消费          |
| Fancyx.Orleans           | Orleans配置,Redis集群,Redis存储                  |
| Fancyx.Redis             | Redis配置，混合缓存类                              |
| Fancyx.Serilog           | Serilog公共配置                                |
| Fancyx.SnowflakeId       | 雪花ID生成                                     |
| Fancyx.Storage           | 对象存储层，目前支持本地服务器存储和阿里云OSS存储两种               |
| Fancyx.Swagger           | SwaggerPro配置，增加了SwaggerGroup特性，支持Swagger分组 |
| Fancyx.Utils             | 工具、帮助类所在                                   |
| Fancyx.Admin             | 模块入口，主机服务                                  |
| Fancyx.Admin.Application | 模块业务层，用于写业务逻辑                              |
| Fancyx.Admin.EfCore      | 模块数据层，包含实体、自定义仓储类                          |
| Fancyx.Gateway.Ocelot    | Ocelot网关，支持直连和Consul服务注册发现                 |
| Fancyx.Shared            | 业务共享的模型、常量                                 |
| Fancyx.Shared.EfCore     | 业务共享的EfCore配置、扩展                           |
| Fancyx.Shared.Logger     | 业务共享的日志层，含业务日志、异常日志、访问日志                   |
| Fancyx.Shared.Proto      | GrpcProto生成共享层，引入后可以直接使用生成的代码              |
| Fancyx.Shared.WebApi     | WebApi共享层，业务权限认证、过滤器等                      |

**前端**

| 文件/目录          | 描述                                                         |
| -------------- | ---------------------------------------------------------- |
| api            | 使用请求库调用接口的封装，可以当做前端接口控制器                                   |
| assets         | 静态资源目录                                                     |
| components     | 公共组件目录                                                     |
| layout         | 后台管理布局组件                                                   |
| pages          | 页面/视图代码（业务代码）                                              |
| router         | react-router封装，静态路由导出、动态路由生成                               |
| store          | 状态存储，1.使用redux+redux-persist实现本地存储，2.使用mobx+localStorage实现 |
| types          | 公共类型定义                                                     |
| utils          | 工具类目录，如：请求库axios封装，全局共享枚举                                  |
| App.tsx        | 应用入口文件，所有组件都经过此入口                                          |
| index.scss     | 全局样式，在这里写的样式，每个组件都能用                                       |
| main.tsx       | 应用启动文件，保活组件域、路由边界、状态机引入等                                   |
| .env.xxx       | 启动命令使用"--mode xxx"来指定                                      |
| vite.config.ts | vite项目配置，由vite脚手架生成                                        |

### 网关配置文件

网关appsettings.json

```json
{
  "ServiceMode": "Direct", //指定模式
  "CorsOrigins": "http://localhost:8080" //跨域配置
}
```

* `ocelot.consul.json`是`ServiceMode`== "Consul"时使用，表示使用consul服务发现
* `ocelot.direct.json`时`ServiceMode`== "Direct"时使用，表示固定IP和端口直连

### Admin配置文件

```json
{
  //consul连接配置，必须需要配置HttpPort和NodeName，其它可选
  "Consul": {
    "Host": "http://localhost:8500",
    "Token": "99abbc21-ddaf-1cd4-a301-51c0df690841",
    "NodeName": "fancyx-admin-api",
    "HttpPort": 5001, //http端口
    "GrpcPort": 50001 //grpc端口，不需要可以不配置
  },
  "Services": {
    //指定consul模式还是直连模式
    "Mode": "Direct", //Direct or Consul
    "Address": [
      {
        "Name": "fancyx-admin-api",
        "Http": "http://localhost:5001",
        "Grpc": "http://localhost:50001"
      },
      {
        "Name": "fancyx-erp-api",
        "Http": "http://localhost:5002",
        "Grpc": "http://localhost:50002"
      }
    ]
  },
  //是否允许一个用户多处同时登录
  "AccountManyLogin": true,
  "ConnectionStrings": {
    "Default": "server=127.0.0.1;uid=root;pwd=123456;database=fancyx-admin"
  },
  "Redis": {
    //Redis连接字符串
    "Connection": "127.0.0.1:6379,password=123456"
  },
  "Cap": {
    "TableSchema": "cap",
    "RedisConnection": "127.0.0.1:6379,password=123456,defaultDatabase=1",
    "DbConnection": "server=127.0.0.1;uid=root;pwd=123456;database=fancyx-admin"
  },
  "Jwt": {
    //JWT误差时间（秒）
    "ClockSkew": 300,
    //JWT发布者
    "ValidAudience": "api",
    //JWT发布者
    "ValidIssuer": "fancyx-admin",
    //JWT签名密钥
    "IssuerSigningKey": "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"
  },
  "Oss": {
    //对象存储路径
    "Bucket": "D:\\Oss",
    //阿里云OSS配置
    "Aliyun": {
      "AccessKey": "", 
      "AccessKeySecret": "",
      "Endpoint": "",
      "Bucket": "",
      "Timeout": 60000,
      "Domain": ""
    },
    "S3": {
      "AccessKey": "minioadmin",
      "SecretKey": "minioadmin",
      "Region": "admin",
      "ServiceURL": "http://localhost:9090"
    }
  },
  "Snowflake": {
    //雪花ID工作ID
    "WorkerId": 1,
    //雪花ID数据中心ID
    "DataCenterId": 4
  },
  "Mqtt": {
    //MQTT服务器暴露端口
    "Port": 1883,
    //MQTT连接账号
    "UserName": "admin",
    //MQTT连接密码
    "Password": "123qwe*"
  }
}
```

### 多租户

1. 每个租户可以配置菜单；

2. 租户配置菜单后，租户下的角色才会有对应的菜单；

3. 目前项目中的做法是：平台租户`platform`用来管理所有租户，如果您需要对租户开新账户，需要先添加租户，分配租户能访问的菜单，再通过租户页面初始化管理员账号按钮进行初始化（初始化可以多次，每次账号不同，都有已分配的租户菜单所有权限）；

4. 租户推荐使用域名绑定，租户中间件会通过域名匹配对应租户ID，实现无感（不用刷新配置/重启网关）开通新租户。

### 权限使用

1. 按钮权限对应的是接口权限，即时生效（如取消某权限，按钮存在，但提交后接口会报无权限），前端的按钮显示隐藏需要重新登录；

2. 目录和菜单对应的是路由和侧边栏，需要重新登录才生效。

### 项目启动

后端项目启动：

* 确认使用Consul服务注册发现还是直连，详见配置：`Services.Mode`
* 修改配置数据库驱动,Redis配置
* 执行根目录下`docs/db/mysql.sql`，会创建表结构，初始化数据
* 修改OSS配置，使用本地目录（盘符一定要有，目录不存在会自动创建）
* 使用VS2022启动网关和你需要的服务（如Fancyx.Admin）

前端项目启动：

* 提前安装`yarn`，运行命令：`npm install -g yarn`
* 安装依赖包，运行命令：`yarn install`
* 开发环境启动，运行命令：`yarn run dev`

## 参与贡献

1. Fork本项目
2. 创建您的特性分支 (git checkout -b feature/AmazingFeature)
3. 提交您的更改 (git commit -m 'Add some AmazingFeature')
4. 推送到分支 (git push origin feature/AmazingFeature)
5. 提交Pull Request

## 许可证

本项目采用[MIT](./LICENSE)开源许可，个人或企业均可免费使用

## 联系方式

* 邮箱：fancyxnet@gmail.com/crackerwork@outlook.com
* QQ：3805712581
* 在线文档： https://doc.crackerwork.cn

## 赞赏列表

*排名按照赞赏时间正序排列*

| 名称    | 金额     |
| ----- | ------ |
| *杰    | 10元    |
| **彬   | 16.8元  |
| Wo**p | 18.88元 |