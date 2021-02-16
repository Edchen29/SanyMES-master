// ***********************************************************************
// <summary>IOC扩展</summary>
// ***********************************************************************

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebRepository;
using IContainer = Autofac.IContainer;

namespace WebApp
{
    public static class AutofacExt
    {
        private static IContainer _container;
        public static IContainer InitAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            services.AddScoped(typeof(IAuth), typeof(LocalAuth));

            //注册app层
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            //缓存注入
            services.AddScoped(typeof(ICacheContext), typeof(CacheContext));
            services.AddScoped(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));

            //筛选器注入
            services.AddScoped<OperLogFilter>();
            services.AddScoped<InterfaceLogFilter>();

            builder.Populate(services);

            _container = builder.Build();
            return _container;
        }

        /// <summary>
        /// 从容器中获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetFromFac<T>()
        {
            return _container.Resolve<T>();
        }
    }
}