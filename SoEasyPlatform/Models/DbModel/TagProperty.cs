using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    /// <summary>
    /// 属性标签，用于模版扩展
    /// </summary>
    public class TagProperty
    {
        [SqlSugar.SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int Id { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string UniueCode { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Description { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string ControlType { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string UrlKey { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Url { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string FileValue { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string FileName { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Ext1 { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Ext2 { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Ext3 { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Ext4 { get; set; }
        [SqlSugar.SugarColumn(IsNullable =true)]
        public string Ext5 { get; set; }
    }
}
