using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class CommonFieldViewModel : PageModel, IView
    {
        [PropertyName("编号")]
        public int? Id { get; set; }
        [ValidateReduired()]
        [PropertyName("类中属性名")]
        public string ClassProperName { get; set; }
        [ValidateReduired()]
        [PropertyName("数据类型")]
        public string CodeType { get; set; }
        [PropertyName("数据库字段名")]
        public string DbColumnName { get; set; }
        public bool? Required { get; set; }
        public bool? IsIdentity { get; set; }
        public bool? IsPrimaryKey { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }
        public int? CodeTableId { get; set; }
    }

    public class CommonFieldGridViewModel 
    {
        [PropertyName("编号")]
        public int Id { get; set; }
        [PropertyName("类中属性名")]
        public string ClassProperName { get; set; }
        [PropertyName("数据类型")]
        public string CodeType { get; set; }
        [PropertyName("数据库字段名")]
        public string DbColumnName { get; set; }
        [PropertyName("必填")]
        public bool? Required { get; set; }
        [PropertyName("自增")]
        public bool? IsIdentity { get; set; }
        [PropertyName("主键")]
        public bool? IsPrimaryKey { get; set; }
        [PropertyName("备注")]
        public string Description { get; set; }
    }
}
