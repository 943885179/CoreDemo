### docker安装skywalking
###### docker 安装服务端
>默认H2存储
`docker run --name skywalking -d -p 1234:1234 -p 11800:11800 -p 12800:12800 --restart always apache/skywalking-oap-server`

> elasticsearch存储
>> 安装ElasticSearch
    `docker run -d --name elasticsearch -p 9200:9200 -p 9300:9300 --restart always -e "discovery.type=single-node" elasticsearch`
>>ElasticSearch管理界面elasticsearch-hq
    `docker run -d --name elastic-hq -p 5000:5000 --restart always elastichq/elasticsearch-hq `
>> 安装
    `docker run --name skywalking -d -p 1234:1234 -p 11800:11800 -p 12800:12800 --restart always --link elasticsearch:elasticsearch -e SW_STORAGE=elasticsearch -e SW_STORAGE_ES_CLUSTER_NODES=elasticsearch:9200 apache/skywalking-oap-server `
>安装管理界面
 `docker run --name skywalking-ui -d -p 8080:8080 --link skywalking:skywalking -e SW_OAP_ADDRESS=skywalking:12800 --restart always apache/skywalking-ui`
### 访问[http://127.0.0.1:8080](http://127.0.0.1:8080)

### .netCore使用skywolking
>安装nuget`SkyAPM.Agent.AspNetCore`

>添加skyapm.json
 ```
 {
  "SkyWalking": {
    "ServiceName": "项目名称",
    "Namespace": "",
    "HeaderVersions": [
      "sw6"
    ],
    "Sampling": {
      "SamplePer3Secs": -1,
      "Percentage": -1.0
    },
    "Logging": {
      "Level": "Debug",
      "FilePath": "logs/skyapm-{Date}.log"
    },
    "Transport": {
      "Interval": 3000,
      "ProtocolVersion": "v6",
      "QueueSize": 30000,
      "BatchSize": 3000,
      "gRPC": {
        "Servers": "127.0.0.1:11800", 
        "Timeout": 10000,
        "ConnectTimeout": 10000,
        "ReportTimeout": 600000
      }
    }
  }
}
 ```
>launchSettings.json添加参数
 ` "SKYWALKING__SERVICENAME": "CoreDemo_Skywalking"`
 和
 `"ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "SkyAPM.Agent.AspNetCore"`
 
 >完整的launchSettings.json
  ```
    {
    "iisSettings": {
        "windowsAuthentication": false,
        "anonymousAuthentication": true,
        "iisExpress": {
        "applicationUrl": "http://localhost:57674",
        "sslPort": 44391
        }
    },
    "$schema": "http://json.schemastore.org/launchsettings.json",
    "profiles": {
        "IIS Express": {
        "commandName": "IISExpress",
        "launchBrowser": true,
        "launchUrl": "api/values",
        "environmentVariables": {
            "SKYWALKING__SERVICENAME": "CoreDemo_Skywalking",
            "ASPNETCORE_ENVIRONMENT": "Development",
            "ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "SkyAPM.Agent.AspNetCore"
        }
        },
        "CoreDemo_Skywalking": {
        "commandName": "Project",
        "launchBrowser": true,
        "launchUrl": "api/values",
        "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development",
            "ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "SkyAPM.Agent.AspNetCore"
        },
        "applicationUrl": "https://localhost:5001;http://localhost:5000"
        },
        "Docker": {
        "commandName": "Docker",
        "launchBrowser": true,
        "launchUrl": "{Scheme}://{ServiceHost}:{ServicePort}/api/values",
        "environmentVariables": {
            "ASPNETCORE_URLS": "https://+:443;http://+:80",
            "ASPNETCORE_HTTPS_PORT": "44392"
        },
        "httpPort": 57684,
        "useSSL": true,
        "sslPort": 44392
        }
    }
    }
  ```
>ctrl+F5启动项目访问api接口，再查看skywalking客户端上是否有接口访问历史（*注意时间选择）
