using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class IndexViewModel : PageViewModel,IView
    {
        [PropertyName("编号")]
        public int? Id { get; set; }
        [PropertyName("备注")]
        [ValidateReduired()]
        public string Desc { get; set; }
        [PropertyName("连接字符串")]
        [ValidateReduired()]
        public string Connection { get; set; }
        [PropertyName("数据库类型")]
        [ValidateReduired()]
        public string DbType { get; set; }
        [PropertyName("更新时间")]
        public DateTime ChangeTime { get; set; }
    }
    public class DBConnectionGridViewModel  
    {
        [DisplayName("编号")]
        public int Id { get; set; }
        [DisplayName("备注")]
        public string Desc { get; set; }
        [DisplayName("连接字符串")]
        public string Connection { get; set; }
        [DisplayName("数据库类型")]
        public string DbType { get; set; }
        [DisplayName("更新时间")]
        public DateTime ChangeTime { get; set; }
    }
}
