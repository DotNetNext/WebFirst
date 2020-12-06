using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
namespace SoEasyPlatform 
{
    public class Nuget
    {
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public int NetVersion { get; set; }

        public bool IsDeleted { get; set; }
    }
}
