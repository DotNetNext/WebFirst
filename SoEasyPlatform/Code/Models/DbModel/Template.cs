using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
namespace SoEasyPlatform 
{
    public class Template
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string Title { get; set; }

        public int TemplateTypeId { get; set; }

        public string Content { get; set; }

        public string TemplateTypeName { get; set; }

        public int Sort { get; set; }

        public DateTime ChangeTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsInit { get; set; }
    }
 
    public class TemplateHistory
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int TemplateTypeId { get; set; }

        public string Content { get; set; }

        public string TemplateTypeName { get; set; }

        public int Sort { get; set; }

        public DateTime ChangeTime { get; set; }
    }
}
