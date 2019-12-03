# .net core Filter 过滤器
## 过滤器（优先级从上到下）
### Authorization Filter(权限过滤器)：继承IAuthorizationFilter 或者IAsyncAuthorizationFilter
### Resource Filter（资源过滤器）:继承IResourceFilter或者IAsyncResourceFilter
### Action Filter （路由过滤器）:继承IActionFilter或者IAsyncActionFilter 或者是 ActionFilterAttribute（其他同理，这个可以直接使用注解来局部引入，不过不是接口没有强制实现方法，所以最好接口也继承或者自己重写对应的方法）
### Exception Filter （异常过滤器）：继承IExceptionFilter或者IAsyncEexceptionFilter
### Result Filter（结果过滤器）：继承IResultFilter或者IAsyncResultFilter 
## 全局注册
	```csharp
		Action<MvcOptions> action = new Action<MvcOptions>(r=> {
                r.Filters.Add(typeof(WeixiaoActionFilter));
        });
        services.AddMvc(action);
		//或者
		services.AddMvc(o => {
            o.Filters.Add<WeixiaoActionFilter>();
        });
	```
## 单个引用
	>方法一：自定义的Filter继承Atrribute,然后使用 `[自定义的Filter类名]`
	>方法二：如果你的过滤器中使用了，构造函数注入对象的形式，直接在控制器上打特性标签就行不通了使用`[TypeFilter(typeof(MyActionFilterAttribute))]` 或者`[ServiceFilter(typeof(MyActionFilterAttribute))] 同时注册services.AddScoped(typeof(MyActionFilterAttribute));`