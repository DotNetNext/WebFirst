using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class FileInfoViewModel:PageViewModel,IView
    {
        [PropertyName("编号")]
        public int? Id { get; set; }
        [ValidateReduired()]
        [PropertyName("类库名")]
        public string Name { get; set; }
        [ValidateReduired()]
        [PropertyName("类库版本")]
        public string Version { get; set; }
        [ValidateReduired()]
        [PropertyName(".Net版本")]
        public int? NetVersion { get; set; }
    }
    public class FileInfoGridViewModel
    {
        [DisplayName("编号")]
        public int Id { get; set; }
        [DisplayName("名称")]

        public string Name { get; set; }
        [DisplayName("填充模版")]
        public string Json { get; set; }
        [DisplayName("文件后缀")]
        public string Suffix { get; set; }
    }
}
