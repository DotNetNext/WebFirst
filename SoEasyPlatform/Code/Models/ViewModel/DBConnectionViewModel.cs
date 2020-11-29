using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class DBConnectionViewModel : PageViewModel
    {
        [DisplayName("编号")]
        public int Id { get; set; }
        [DisplayName("连接字符串")]
        public string Connection { get; set; }
        [DisplayName("数据库类型")]
        public string DbType { get; set; }
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }
    }
    public class DBConnectionGridViewModel  
    {
        [DisplayName("编号")]
        public int Id { get; set; }
        [DisplayName("连接字符串")]
        public string Connection { get; set; }
        [DisplayName("数据库类型")]
        public string DbType { get; set; }
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
