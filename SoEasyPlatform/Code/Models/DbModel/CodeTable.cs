using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
namespace SoEasyPlatform 
{
    public class CodeTable
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int DbId { get; set; }
        public string ClassName { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime PreUpdateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsLock { get; set; }
    }
    public class CodeColumns
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string ClassProperName { get; set; }
        public string DbColumnName { get; set; }
        public bool Required { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string Description { get; set; }
        public string CodeType { get; set; }
        public int CodeTableId { get; set; }

    }
}
