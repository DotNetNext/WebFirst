using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class MapperProfiles : AutoMapper.Profile
    {
        public MapperProfiles()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var item in types)
            {
                if (types.Any(it => it.Name != item.Name && it.Name.Contains(item.Name)))
                {
                    var mapTypes = types.Where(it => it.Name != item.Name && it.Name.Contains(item.Name));
                    foreach (var mapType in mapTypes)
                    {
                        CreateMap(item, mapType);
                        CreateMap(mapType, item);
                    }
                }
            }
        }
    }
}
