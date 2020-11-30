using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class TableViewModel : PageViewModel, IView
    {
        [PropertyName("表名")]
        public string TableName { get; set; }
        [PropertyName("备注")]
        public string TableDesc { get; set; }
        [PropertyName("数据库")]
        public int Database { get; set; }
    }

    public class TableGridViewModel  
    {
        [PropertyName("表名")]
        public string TableName { get; set; }
        [PropertyName("备注")]
        public string TableDesc { get; set; }
        [PropertyName("数据库")]
        public string Database { get; set; }
    }
}
