using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
namespace SoEasyPlatform 
{
    public class ProjectViewModel 
    {
        public int? Id { get; set; }
        public string TemplateId1 { get; set; }
        public string Path { get; set; }
        public string NetVersion { get; set; }
        public string LibraryName { get; set; }
        public string Nuget { get; set; }
        public string ProjentName { get; set; }
        public int? Sort { get; set; }
        public bool? IsDeleted { get; set; }
        public string Tables { get; set; }
    }
}
