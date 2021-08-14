using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SqlSugar.IOC;
namespace SoEasyPlatform
{
    /// <summary>
    /// 启动入口
    /// </summary>
    public class Startup
    {
        #region 配置参数
        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version = "124";
        /// <summary>
        /// 接口域名目录
        /// </summary>
        /// <param name="configuration"></param>
        public static string RootUrl = "/api/"; 
        #endregion

        #region 配置方法
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 配置对象
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Services.AddServices(services);
            services.AddSqlSugar(new SqlSugar.IOC.IocConfig()
            {
                ConfigId = "master1",
                DbType = IocDbType.Sqlite,
                IsAutoCloseConnection = true,
                ConnectionString = "DataSource=" + AppContext.BaseDirectory + @"\database\sqlite.db"
            });
            services.ConfigurationSugar(db =>
            {
                if (!db.ConfigQuery.Any())
                {
                    db.ConfigQuery.SetTable<Template>(it => it.Id, it => it.Title);
                    db.ConfigQuery.SetTable<TemplateType>(it => it.Id, it => it.Name);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Configures.AddConfigure(app, env);
        } 
        #endregion
    }
}
