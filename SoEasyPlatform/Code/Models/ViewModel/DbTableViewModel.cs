using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class DbTableViewModel
    {
        [DisplayName("表名")]
        public string Name
        {
            get;
            set;
        }
        [DisplayName("备注")]
        public string Description
        {
            get;
            set;
        }

        [DisplayName("状态")]
        public string IsImport
        {
            get;
            set;
        }

    }
}
