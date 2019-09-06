### 简单的使用Mysql
###### Nuget :Mysql.data
>开始使用；
 ```
  using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Mysql
{
    public class MysqlHelp
    {
        public string ConnectionString { get; set; }

        public MysqlHelp(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public object GetAllCountry()
        {
            //连接数据库
            using (MySqlConnection msconnection = GetConnection())
            {
                msconnection.Open();
                //查找数据库里面的表
                MySqlCommand mscommand = new MySqlCommand("select * from user", msconnection);
                using (MySqlDataReader reader = mscommand.ExecuteReader())
                {
                    //读取数据
                    while (reader.Read())
                    {
                        var ss = reader.GetString("a");
                        //list.Add(new Country()
                        //{
                        //    Code = reader.GetString("Code"),
                        //    Name = reader.GetString("Name"),
                        //    Continent = reader.GetString("Continent"),
                        //    Region = reader.GetString("Region")
                        //});
                    }
                }
            }
            return null;
        }
    }
}
 ```
> 注入`var op = Configuration.GetConnectionString("Default");services.Add(new ServiceDescriptor(typeof(MysqlHelp), new MysqlHelp(op)));`
> 调用
 ```
     MysqlHelp context = HttpContext.RequestServices.GetService(typeof(MysqlHelp)) as MysqlHelp;
     var x=context.GetAllCountry();
 ```

### EF
###### Nuget :Mysql.data、Mysql.data.EntityFramworkCore、EntityFramworkCore
> 创建dbcontxt
 ```
  
    public class MyDbContext:DbContext
    {
        public DbSet<user> user { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
    }
 ```
> 依赖注入 ` var op = Configuration.GetConnectionString("Default");services.AddDbContext<MyDbContext>(options => options.UseMySQL(op));`
> 调用
 ```
        private MyDbContext db;

        public ValuesController(MyDbContext db)
        {
            this.db = db;
        }
       [HttpGet]
        public object Get()
        {
           return db.user.ToList();
        }
 ```
### dapper 
###### Nuget: Mysql.data、dapper、（其他看个人习惯，Z.Dapper.Plus批量操作，Dapper.SimpleCRUD 和 dapper.SimpleSave和dapper.contrib 选择一个即可本次使用的是dapper.contrib）
>创建DbContex
>> 方法一
 ```
  
    public interface ISqlHelper
    {
        IDbConnection GetConnection();
    }
    public class SqlHelper : ISqlHelper
    {
        private IConfiguration _configuration;
        private static IDbConnection connection;
        public SqlHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            var configString = _configuration.GetConnectionString("Default");
            if (configString.ToLower().Contains("charset") || configString.ToLower().Contains("port"))
            {
                connection = new MySqlConnection(configString);
            }
            else
            {

                connection = new SqlConnection(configString);
            }
        }
        public IDbConnection GetConnection()
        {
            return connection;
        }
    }
 ```
 >>>注入Startup.cs`services.AddSingleton<ISqlHelper, SqlHelper>();`
 >>>使用
  ```
        private ISqlHelper helper;
        private static IDbConnection sdb;
        public ValuesController(ISqlHelper db)
        {
            this.helper = db;
            sdb = helper.GetConnection();
        }
         [HttpGet]
        public object Get()
        {
            return sdb.Query("select * from user").ToList();
        }
  ```
---
>> 方法二
 ```
   public class MyDbContext
    {
        public static IDbConnection Connection;
        public MyDbContext(string configString)
        {
            if (configString.ToLower().Contains("charset") || configString.ToLower().Contains("port"))
            {
                Connection = new MySqlConnection(configString);
            }
            else
            {

                Connection = new SqlConnection(configString);
            }
        }
    }
 ```
 >>>注入Startup.cs` services.Add(new ServiceDescriptor(typeof(MyDbContext), new MyDbContext(Configuration.GetConnectionString("Default"))));`
 >>>使用
  ```
         private static IDbConnection mdb = MyDbContext.Connection;
         [HttpGet]
        public object Get()
        {
            return mdb.Query("select * from user").ToList();
        }
  ```
---
###### dapper.contrib 的注意事项  需要给实体添加[Table("tableName")]属性，否自会以复数的形式访问数据库，必须有[Key]自增或者[ExplicitKey]主键不自增属性，需要自行判断，自增时候给该列赋值无效，非自增时候有效，需要排除部分字段用[Write(false)],
###### datper.plus 批量操作注意事项，必须提前声明自增表名等，否则自增属性无效`DapperPlusManager.Entity<Person>().Table("Person").Identity(o => o.Id);`这样才能对一对一一对多批量更新添加等操作，然后就是顺序，需要有先后顺序

