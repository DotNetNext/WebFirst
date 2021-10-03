using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    /// <summary>
    /// 属性标签，用于模版扩展
    /// </summary>
    public class TagPropertyViewModel : PageViewModel, IView
    {
        public int? Id { get; set; }
        [ValidateReduired()]
        [PropertyName("标识 UniueCode")]
        [ValidateUnique("TagProperty", "UniueCode", "id")]
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


    /// <summary>
    /// 属性标签，用于模版扩展
    /// </summary>
    public class TagPropertyGridViewModel : PageViewModel, IView
    {
        [DisplayName("主键")]
        public int? Id { get; set; }
        [DisplayName("唯一标识")]
        public string UniueCode { get; set; }
        [DisplayName("描述")]
        public string Description { get; set; }
        [DisplayName("控件类型")]
        public string ControlType { get; set; }
        [DisplayName("接口 Key")]
        public string UrlKey { get; set; }
        [DisplayName("接口 地址")]
        public string Url { get; set; }
        [DisplayName("字段值")]
        public string FileValue { get; set; }
        [DisplayName("字段名")]
        public string FileName { get; set; }
        [DisplayName("扩展1")]
        public string Ext1 { get; set; }
        [DisplayName("扩展2")]
        public string Ext2 { get; set; }
        [DisplayName("扩展3")]
        public string Ext3 { get; set; }
        //public string Ext4 { get; set; }
        //public string Ext5 { get; set; }
    }
}
