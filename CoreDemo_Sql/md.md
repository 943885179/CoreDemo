> 配置SqlServer
- Nuget安装 
   + Microsoft.EntityFrameworkCore 
   + Microsoft.EntityFrameworkCore.SqlServer 
   + Microsoft.EntityFrameworkCore.SqlServer.Design 
   + Microsoft.EntityFrameworkCore.Tools 
- appsettings.json配置数据库连接
    
    ```
    "ConnectionStrings": {
    "SQLConnection": "Server=.;Database=CustomerDB;Trusted_Connection=True;"
    // "Server=.;Database=dbCore;User ID=sa;Password=abc123456;"
    },
- 添加实体
   ```
    public class User{
        [Key]
        public int Id{get;set;}
        public string Name{get;set;} 
    }
- 添加DbConText
  ```
  public class MyDbContext:DbContext{
        public DbSet<User> User { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
  }
- 注册DbContext到Startup.cs中
    ```
     services.AddDbContext<MyDbContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("SQLConnection")));
- 更新实体到数据库中
    ```
    //打开程序管理控制台输入命令
    Add-Migration [Name]
    update-database [Name]
    执行完后可以去数据库看看是否生成了对应的数据库（第一次生成，后面修改）
- 再控制器中调用EF
 ```
   public class ValuesController : ControllerBase
    {
        private readonly static MyDbContext _db;
        public ValuesController(MyDbContext db) {
            _db = db;
        }

        // GET api/values
        [HttpGet]
        public Object Get()
        {
           // var result = _db.Set<User>().ToList();
            var result = _db.User.ToList();
            return result;
        }
    }
```
- 其他操作:重新DbContext的OnModelCreating方法修改表名，字段大小，主外键关系等
[链接](https://blog.csdn.net/slowlifes/article/details/9359141)
