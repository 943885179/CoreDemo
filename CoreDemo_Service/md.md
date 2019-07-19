>定义接口和实现类
 ```
    public interface IUser
    {
        string GetName(string name);
    }
    
    public class User : IUser
    {
        public string GetName(string name)
        {
            return name;
        }
    }
 ```
> 注入服务
 ```
  services.AddTransient<IUser, User>();//每次请求时创建。 最好用于轻量级无状态服务,每次从容器 （IServiceProvider）中获取的时候都是一个新的实例。
 ```
+ AddTransient瞬时模式：每次请求，都获取一个新的实例。即使同一个请求获取多次也会是不同的实例
+ AddScoped：每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例
+ AddSingleton单例模式：每次都获取同一个实例