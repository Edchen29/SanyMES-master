using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using WebApp;
using WebRepository;

namespace WebMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
                options.KeyLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartBoundaryLengthLimit = int.MaxValue;
            });

            services.AddMvc(option =>
            {
                option.ModelBinderProviders.Insert(0, new JsonBinderProvider());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMemoryCache();
            services.AddOptions();

            //映射配置文件
            services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));
            //解密数据库连接字符串的密码
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("BaseDBContext"));
            sqlConnectionStringBuilder.Password = Encryption.Decrypt(sqlConnectionStringBuilder.Password);

            services.AddDbContext<BaseDBContext>(options =>
                //SQL2008启用分页支持
                options.UseSqlServer(sqlConnectionStringBuilder.ConnectionString, b => b.UseRowNumberForPaging())
            );

            #region 启用Quartz中间件
            services.AddQuartz(options =>
            {
                options.UseSqlServer(sqlConnectionStringBuilder.ConnectionString);
                options.UseProperties(false);
            });
            #endregion

            //注册数据库基础操作和工作单元
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUnitWork), typeof(UnitWork));
            services.AddScoped(typeof(ISqlWork), typeof(SqlWork));

            #region 添加Swagger中间件
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "华恒API",
                    Description = "华恒API接口",
                });
                //注入WebAPI注释文件给Swagger  
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "WebMvc.xml");
                options.IncludeXmlComments(xmlPath, true);

                var assembly = Assembly.GetAssembly(typeof(BaseDBContext));
                options.IncludeXmlComments(assembly.Location.Replace("dll", "xml"));

                options.IgnoreObsoleteActions();
            });
            #endregion

            services.AddSignalR();

            //使用AutoFac进行注入
            return new AutofacServiceProvider(AutofacExt.InitAutofac(services));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            //添加静态文件中间件
            app.UseStaticFiles();

            #region 启用Swagger中间件
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.ShowExtensions();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "华恒API接口文档");
                c.InjectStylesheet("/css/swagger.css");
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            });
            #endregion

            #region 启用Quartz中间件
            app.UseQuartz();
            #endregion

            app.UseMvcWithDefaultRoute();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChartHub>("/ChartHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
