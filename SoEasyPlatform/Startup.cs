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

namespace SoEasyPlatform
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,

                builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")

                );

            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddRazorPages();

            services.AddAutoMapper(typeof(SoEasyPlatform.MapperProfiles).Assembly);

#if DEBUG
            //启用动态编译
            services.AddControllersWithViews()
            .AddRazorRuntimeCompilation();
#endif
            services.AddControllersWithViews().AddNewtonsoftJson(opt =>
            {
                //忽略循环引用
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                //不改变字段大小
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

          
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.InjectJavascript("");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });

            InitTable.Start();
        }
    }
}
