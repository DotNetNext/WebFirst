using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class Services
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddControllers();
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
    }
}
