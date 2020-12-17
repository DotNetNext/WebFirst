using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
namespace SoEasyPlatform 
{
    public class CodeType
    {
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CSharepType { get; set; }
        [SugarColumn(ColumnDataType = "text", IsJson = true)]
        public DbTypeInfo[] DbType { get; set; } 

        public int Sort { get; set; }
    }
    public class DbTypeInfo 
    {
        public string Name { get; set; }
        public int? Length { get; set; }
        public int? DecimalDigits { get; set; }
    }
}
