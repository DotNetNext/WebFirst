using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform
{
    public class TemplateViewModel : PageViewModel, IView
    {

        [PropertyName("编号")]
        public int? Id { get; set; }
        [PropertyName("排序")]
        public int? Sort { get; set; }
        [PropertyName("名称")]
        public string Title { get; set; }
        [PropertyName("类型")]
        public int? TemplateTypeId { get; set; }
        [PropertyName("内容")]
        public string Content { get; set; }
        [PropertyName("类型名称")]
        public string TemplateTypeName { get; set; }
    }

    public class TemplateGridViewModel
    {
        [DisplayName("编号")]
        public int Id { get; set; }
        [DisplayName("排序")]
        public int Sort { get; set; }
        [DisplayName("名称")]
        public string Title { get; set; }
        [DisplayName("类型名称")]
        public string TemplateTypeName { get; set; }
        [DisplayName("类型")]
        public int? TemplateTypeId { get; set; }
        [DisplayName("内容")]
        public string Content { get; set; }
        [DisplayName("更新时间")]
        public DateTime ChangeTime { get; set; }
    }
}
