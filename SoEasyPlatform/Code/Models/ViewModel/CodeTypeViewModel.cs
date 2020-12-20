using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class CodeTypeViewModel:PageModel,IView
    {
        [PropertyName("编号")]
        public int? Id { get; set; }
        [ValidateReduired()]
        [PropertyName("类型名称")]
        public string Name { get; set; }
        [ValidateReduired()]
        [ValidateWord()]
        [PropertyName("C#类型")]
        public string CSharepType { get; set; }
        [ValidateReduired()]
        [PropertyName("数据类型")]
        public string DbType { get; set; }
        [PropertyName("排序")]
        [ValidateInt()]
        public int? Sort { get; set; }
    }
    public class CodeTypeGridViewModel
    {
        [DisplayName("编号")]
        public int Id { get; set; }
        [DisplayName("排序")]
        public int Sort { get; set; }
        [DisplayName("类型名称")]
        public string Name { get; set; }
        [DisplayName("C#类型")]
        public string CSharepType { get; set; }
        [DisplayName("数据类型")]
        public string DbType { get; set; }
    }
}
