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
        [SugarColumn(IsNullable = true)]
        public string FileInfo { get; set; }
        [SugarColumn(IsNullable =true)]
        public string FileModel { get; set; }
        public int Sort { get; set; }
        public bool IsDeleted { get; set; }
        public int ModelId { get; set; }
        public string FileSuffix { get;  set; }
        public string ProjentName { get;  set; }
        [SugarColumn(IsNullable =true,Length =50)]
        public string SolutionId { get; set; }
        [SugarColumn(IsNullable = true)]
        public bool? IsInit { get; set; }
    }

 
}
