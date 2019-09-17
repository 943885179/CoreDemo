>下载Nuget包 Autofac 和 Autofac.Extensions.DependencyInjection

>添加接口和实现类
  ```csharp
    public interface IAnimal
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        int GetArg(int arg);
    }
    public class Dog : IAnimal
    {
        public string Name {
            get { return "Dog"; }
        }

        public int GetArg(int arg)
        {
            return arg;
        }
    }
    public class Cat : IAnimal
    {
        public string Name
        {
            get { return "Cat"; }
        }
        public int GetArg(int arg)
        {
            return arg;
        }
    }
  ```
> 修改startup.cs 方法中的ConfigureServices 返回IServiceProvider
  ```csharp
   public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();//使用内存缓存
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            return RegisterAutofac(services);//注册Autofac
        }
  ```
> 实例化容器且用Autofac替换.net Core自带容器
 ```csharp
         private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            //实例化Autofac容器
            var builder = new ContainerBuilder();
            //将Services中的服务填充到Autofac中
            builder.Populate(services);
            //新模块组件注册    
            builder.RegisterModule<AutofacModuleRegister>();
            //创建容器
            var Container = builder.Build();
            //第三方IOC接管 core内置DI容器 
            return new AutofacServiceProvider(Container);
        }
 ```
>将实体注入容器中
 ```csharp
   public class AutofacModuleRegister : Autofac.Module
        {
            //重写Autofac管道Load方法，在这里注册注入
            protected override void Load(ContainerBuilder builder)
            {
                //注册Cat指定为IAnimal实现
                //builder.RegisterType<Cat>().As<IAnimal>();
                //builder.RegisterType<Dog>().As<IAnimal>();
                builder.RegisterAssemblyTypes(typeof(Startup).Assembly).AsImplementedInterfaces(); //这里的AsImplementedInterfaces表示以接口的形式注册

                //单独注册
                builder.RegisterType<Cat>().Named<IAnimal>(typeof(Cat).Name);
                builder.RegisterType<Dog>().Named<IAnimal>(typeof(Dog).Name);

                builder.RegisterAssemblyTypes(GetAssemblyByName("BLLS")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
            }
            /// <summary>
            /// 根据程序集名称获取程序集
            /// </summary>
            /// <param name="AssemblyName">程序集名称</param>
            /// <returns></returns>
            public static Assembly GetAssemblyByName(String AssemblyName)
            {
                return Assembly.Load(AssemblyName);
            }
        }
 ```
 - 单个实体注册:`builder.RegisterType<Cat>().As<IAnimal>();` 多个实现类继承同一个接口后面注册的将覆盖前面注册
 - 批量注册 ` builder.RegisterAssemblyTypes(typeof(Startup).Assembly).AsImplementedInterfaces(); //这里的AsImplementedInterfaces表示以接口的形式注册`
 - 多个实现类继承同一个接口需要单独注册 `builder.RegisterType<Cat>().Named<IAnimal>(typeof(Cat).Name);`
>使用
 - 不包含多继承的
    ```csharp
    public class ValuesController : ControllerBase
    {
        IAnimal animal;
        //重写注入
        public ValuesController(IAnimal _animal)
        {
            animal = _animal;
        }
        //调用
         [HttpGet("{id}")]
        public ActionResult<string> Get(string name)
        {
            var result = animal.GetArg(10);
            return result.ToString();
        }
    }
    ```
 - 包含多继承的
    ```csharp
    public class ValuesController : ControllerBase
    {
        IAnimal cat;
        IAnimal dog;
        public ValuesController(IAnimal _animal, IComponentContext componentContext)
        {
            cat = componentContext.ResolveNamed<IAnimal>(typeof(Cat).Name);//分别调用
            dog = componentContext.ResolveNamed<IAnimal>(typeof(Cat).Name);
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string name)
        {
            return dog.Name;
        }
    }
    ```
>其他资源 [中文文档](https://autofaccn.readthedocs.io/zh/latest/)