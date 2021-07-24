using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
namespace SoEasyPlatform 
{
    public class Project 
    {
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int Id { get; set; }
        public string TemplateId1 { get; set; }
        public string Path { get; set; }
        [SugarColumn(IsNullable =true)]
        public string NetVersion { get; set; }
        [SugarColumn(IsNullable = true)]
        public string LibraryName { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Nuget { get; set; }
        [SugarColumn(IsNullable = true)]
        public string ProjentName { get; set; }
        public int Sort { get; set; }
        public bool IsDeleted { get; set; }
        public int ModelId { get; set; }
        public string FileSuffix { get; internal set; }
    }

 
}
