using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
namespace SoEasyPlatform
{
    public class Menu
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int ParentId { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Icon { get; set; }
        public string MenuName { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Url { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<Menu> Child { get; set; }

    }
}
