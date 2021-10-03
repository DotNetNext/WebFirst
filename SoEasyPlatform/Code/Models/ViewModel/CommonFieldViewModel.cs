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
        [ValidateUnique("CommonField", "ClassProperName", "id")]
        public string ClassProperName { get; set; }
        [ValidateReduired()]
        [PropertyName("数据类型")]
        public string CodeType { get; set; }
        [ValidateUnique("CommonField", "DbColumnName", "id")]
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
        [DisplayName("编号")]
        public int Id { get; set; }
        [DisplayName("类中属性名")]
        public string ClassProperName { get; set; }
        [DisplayName("数据类型")]
        public string CodeType { get; set; }
        [DisplayName("数据库字段名")]
        public string DbColumnName { get; set; }
        [DisplayName("必填")]
        public bool? Required { get; set; }
        [DisplayName("自增")]
        public bool? IsIdentity { get; set; }
        [DisplayName("主键")]
        public bool? IsPrimaryKey { get; set; }
        [DisplayName("备注")]
        public string Description { get; set; }
    }
}
