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

            //ӳ�������ļ�
            services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));
            //�������ݿ������ַ���������
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("BaseDBContext"));
            sqlConnectionStringBuilder.Password = Encryption.Decrypt(sqlConnectionStringBuilder.Password);

            services.AddDbContext<BaseDBContext>(options =>
                //SQL2008���÷�ҳ֧��
                options.UseSqlServer(sqlConnectionStringBuilder.ConnectionString, b => b.UseRowNumberForPaging())
            );

            #region ����Quartz�м��
            services.AddQuartz(options =>
            {
                options.UseSqlServer(sqlConnectionStringBuilder.ConnectionString);
                options.UseProperties(false);
            });
            #endregion

            //ע�����ݿ���������͹�����Ԫ
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUnitWork), typeof(UnitWork));
            services.AddScoped(typeof(ISqlWork), typeof(SqlWork));

            #region ���Swagger�м��
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "����API",
                    Description = "����API�ӿ�",
                });
                //ע��WebAPIע���ļ���Swagger  
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "WebMvc.xml");
                options.IncludeXmlComments(xmlPath, true);

                var assembly = Assembly.GetAssembly(typeof(BaseDBContext));
                options.IncludeXmlComments(assembly.Location.Replace("dll", "xml"));

                options.IgnoreObsoleteActions();
            });
            #endregion

            services.AddSignalR();

            //ʹ��AutoFac����ע��
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
            //��Ӿ�̬�ļ��м��
            app.UseStaticFiles();

            #region ����Swagger�м��
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.ShowExtensions();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "����API�ӿ��ĵ�");
                c.InjectStylesheet("/css/swagger.css");
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            });
            #endregion

            #region ����Quartz�м��
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
