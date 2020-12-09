using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class CodeTableViewModel:PageViewModel,IView
    {
        [PropertyName("编号")]
        public int? Id { get; set; }
        [PropertyName("数据库")]
        public int? DbId { get; set; }
        [PropertyName("类名")]
        public string ClassName { get; set; }
        [PropertyName("表名")]
        public string TableName { get; set; }
        [PropertyName("备注")]
        public string Description { get; set; }
    }
    public class CodeTableGridViewModel  
    {
        [DisplayName("编号")]
        public int? Id { get; set; }
        [DisplayName("数据库")]
        public string DbName { get; set; }
        [DisplayName("类名")]
        public string ClassName { get; set; }
        [DisplayName("表名")]
        public string TableName { get; set; }
        [DisplayName("备注")]
        public string Description { get; set; }
    }
}
