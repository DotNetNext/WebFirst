using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class CommonField
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string ClassProperName { get; set; }
        public string DbColumnName { get; set; }
        public bool Required { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }
        public string CodeType { get; set; }
        public int CodeTableId { get; set; }
    }
}
