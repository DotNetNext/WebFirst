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
        public string UniueCode { get; set; }
        public string Description { get; set; }
        public string ControlType { get; set; }
        public string UrlKey { get; set; }
        public string Url { get; set; }
        public string FileValue { get; set; }
        public string FileName { get; set; }
        public string Ext1 { get; set; }
        public string Ext2 { get; set; }
        public string Ext3 { get; set; }
        public string Ext4 { get; set; }
        public string Ext5 { get; set; }
    }
}
