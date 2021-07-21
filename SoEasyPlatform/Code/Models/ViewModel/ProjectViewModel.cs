using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
namespace SoEasyPlatform 
{
    public class ProjectViewModel : PageViewModel, IView
    {
        public int? Id { get; set; }
        [ValidateReduired()]
        [PropertyName("模版")]
        public string TemplateId1 { get; set; }
        [ValidateReduired()]
        [PropertyName("路径")]
        public string Path { get; set; }
        public string NetVersion { get; set; }
        public string LibraryName { get; set; }
        public string Nuget { get; set; }
        public string ProjentName { get; set; }
        [ValidateReduired()]
        [PropertyName("文件后缀")]
        public string FileSuffix{get;set;}
        public int? Sort { get; set; }
        public bool? IsDeleted { get; set; }
        public string Tables { get; set; }
        public int? ProjectId { get; set; }
        public int ModelId { get;  set; }
    }
    public class ProjectGridViewModel  
    {
        [DisplayName("编号")]
        public int? Id { get; set; }
        [DisplayName("方案名")]
        public string ProjentName { get; set; }
        [DisplayName("分类")]
        public string ModelId { get; set; }
        [DisplayName("模版")]
        public string TemplateId1 { get; set; }
        [DisplayName("路径")]
        public string Path { get; set; }
        [DisplayName("路径")]
        public string NetVersion { get; set; }
        [DisplayName("类库")]
        public string LibraryName { get; set; }
        [DisplayName("Nuget")]
        public string Nuget { get; set; }
        [DisplayName("文件后缀")]
        public string FileSuffix { get; set; }
    }
}
