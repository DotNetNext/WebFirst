using System;
using System.Collections.Generic;
using System.Text;

namespace SugarSite.Enties
{
    public class PageViewModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageTotal { get; set; }
    }
}
