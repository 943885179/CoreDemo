> 按照Nuget `Swashbuckle.AspNetCore` 
> startuo.cs注入
 ```
   services.AddSwaggerGen(options =>
            {
                //配置第一个Doc
                options.SwaggerDoc("v1", new Info { Title = "My API_1", Version = "v1" });
                //配置第二个Doc
                options.SwaggerDoc("v2", new Info { Title = "My API_2", Version = "v2" });
            });
   app.UseSwagger();
   app.UseSwaggerUI(options => 
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
            options.RoutePrefix = "swagger";
         });
 ```
> 输入http://localhost:<port>/swagger 查看是否配置成功
> 启用注释功能
 - 打开项目属性，生成选项中选择生成xml文件
 - 为 Swagger JSON and UI设置xml文档注释路径
   ```
    services.AddSwaggerGen(options =>
            {
                //配置第一个Doc
                options.SwaggerDoc("v1", new Info { Title = "My API_1", Version = "v1" });
                //配置第二个Doc
                options.SwaggerDoc("v2", new Info { Title = "My API_2", Version = "v2" });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "CoreDemo_Swagger.xml");
                options.IncludeXmlComments(xmlPath);
            });
   ```
> 添加作者、许可证、说明等信息
 ```
services.AddSwaggerGen(options =>
            {
                //配置第一个Doc
                options.SwaggerDoc("v1", new Info
                {
                    Title = "My API_1",
                    Version = "v1",
                    Description = "这是说明信息",//说明
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "mzj",
                        Email = "943885179@qq.com",
                        Url = "https://github.com/943885179/CoreDemo"
                    },
                    License = new License
                    {
                        Name = "许可证名字",
                        Url = "https://github.com/943885179/CoreDemo"
                    }
                });
                //配置第二个Doc
                options.SwaggerDoc("v2", new Info { Title = "My API_2", Version = "v2" });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "CoreDemo_Swagger.xml");
                options.IncludeXmlComments(xmlPath);
            });
 ```
