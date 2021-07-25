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
        public string FileInfo { get; set; }
        public string FileModel { get; set; }
        [ValidateReduired()]
        [PropertyName("文件后缀")]
        public string FileSuffix{get;set;}
        public int? Sort { get; set; }
        public bool? IsDeleted { get; set; }
        public string Tables { get; set; }
        public int? ProjectId { get; set; }
        public int? ModelId { get;  set; }
        public string ProjentName { get;  set; }
    }
    public class ProjectViewModel2 : PageViewModel, IView
    {
        public string Tables { get; set; }
        public int? ProjectId { get; set; }
        public int? ModelId { get; set; }
    }
    public class ProjectGridViewModel  
    {
        [DisplayName("编号")]
        public int? Id { get; set; }
        [DisplayName("方案名")]
        public string ProjentName { get; set; }
        [DisplayName("追加文件")]
        public string FileInfo { get; set; }
        [DisplayName("填充文件")]
        public string FileModel { get; set; }
        [DisplayName("分类")]
        public string ModelId { get; set; }
        [DisplayName("模版")]
        public string TemplateId1 { get; set; }
        [DisplayName("路径")]
        public string Path { get; set; }
        [DisplayName("文件后缀")]
        public string FileSuffix { get; set; }
    }
}
