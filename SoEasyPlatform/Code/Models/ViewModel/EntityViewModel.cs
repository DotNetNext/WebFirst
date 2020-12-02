using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class EntityViewModel : PageViewModel, IView
    {
        [PropertyName("表名")]
        public string TableName { get; set; }
        [PropertyName("备注")]
        public string TableDesc { get; set; }
        [PropertyName("数据库")]
        public int Database { get; set; }
    }

    public class EntityGridViewModel  
    {

        [DisplayName("编号")]
        public int Id { get; set; }
        [DisplayName("表名")]
        public string TableName { get; set; }
        [DisplayName("备注")]
        public string TableDesc { get; set; }
        [DisplayName("数据库")]
        public string Database { get; set; }
    }

    public class TableToTemplateViewModel:IView
    {
        [PropertyName("模版类型")]
        [ValidateReduired()]
        public int? TemplateId1 { get; set; }
        [PropertyName("模版类型")]
        public int? TemplateId2 { get; set; }
        [ValidateReduired()]
        [PropertyName("路径")]
        public string Path { get; set; }
        [PropertyName("项目名")]
        public string ProjectName { get; set; }
        [PropertyName("格式")]
        public string FileFormat { get; set; }
        [PropertyName("数据表")]
        public string Tables { get; set; }
        public string NetVersion { get; set; }
        public string LibraryName { get; set; }
        public string Nuget { get; set; }
    }
}
