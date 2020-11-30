using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class TemplateViewModel : PageViewModel, IView
    {
        [PropertyName("模版类型")]
        public string TableName { get; set; }
    }

    public class TemplateGridViewModel
    {
        [PropertyName("模版名")]
        public string Title { get; set; }
        [PropertyName("模版类型")]
        public string TemplateType { get; set; }
        [PropertyName("排序号")]
        public  int Sort  { get; set; }
    }
}
