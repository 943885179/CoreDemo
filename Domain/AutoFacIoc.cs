using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using IContainer = Autofac.IContainer;

namespace Domain
{
    public class AutoFacIoc
    {
        public static IContainer Injection(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            //InstancePerLifetimeScope：同一个Lifetime生成的对象是同一个实例
            //SingleInstance：单例模式，每次调用，都会使用同一个实例化的对象；每次都用同一个对象；
            //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；

            //获取IAutoInject的类型
            var baseTypeDomain = typeof(Domain.IAutoInject);
            //获取所有需要依赖注入的程序集
            //DDD.Domain是服务所在程序集命名空间  
            Assembly assembliesDomain = Assembly.Load("Domain");
            //自动注册接口
            builder.RegisterAssemblyTypes(assembliesDomain)
                .Where(b => b.GetInterfaces().Any(c => c == baseTypeDomain && b != baseTypeDomain))
                .AsImplementedInterfaces()
                .SingleInstance(); //见上方说明

            builder.Populate(services);
            return builder.Build();
        }
    }
}
