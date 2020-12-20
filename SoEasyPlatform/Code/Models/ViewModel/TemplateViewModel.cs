using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class TemplateViewModel : PageViewModel, IView
    {
        
            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int Id { get; set; }

            public string Title { get; set; }
            public int TemplateTypeId { get; set; }

            public string Content { get; set; }

            public string TemplateTypeName { get; set; }

            public int Sort { get; set; }

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
