WebServerSolution: 

# 确保开发环境版本一致
1. nodejs 版本: v8.4.0  (npm: 5.3.0)  (https://npm.taobao.org/mirrors/node/v8.4.0/)
2. dotnet core sdk版本: 2.0.2  (https://www.microsoft.com/net/download/core)
3. IIS部署: dotnet core runtime: 2.0.0  (https://www.microsoft.com/net/download/core#/runtime) 



# 启动:
1. cd WebServerSolution/MKServerWeb
2. npm install 
3. dotnet run

# 遇到错误
1. 
```
Node Sass could not find a binding for your current environment: Windows 64-bit with Node.js 8.x
```
> node-sass:  npm rebuild node-sass
2. oracledb
> 安装客户端，配置环境变量

# 发布:
1. dotnet publish -c Release


# element-ui:
修改node_modules/@types/element-ui/index.d.ts
declare module "element-ui";

修改node_modules/@types/echarts/index.d.ts
declare module "echarts";


# oracledb:
1. 环境配置: 下载oracle对应版本的客户端，安装在C:\oracle\instantclient (测试用11.0.2.0)
2. npm uninstall oracledb
3. npm install oracledb --save


# 公安项目部署在IIS
1. 需要安装环境:  
    1. nodejs版本: v6.11.2
2. 在IIS网站根目录下
    1. Node 文件夹
    2. node_modules文件夹 
        1. oracledb
        2. mysql
    3. wwwroot文件夹
3. oracledb和mysql的 npm包在
4. oracle 公安客户端版本: x64-12.2.0.1.0