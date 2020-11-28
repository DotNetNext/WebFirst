using System;
using System.Collections.Generic;
using System.Text;

namespace SugarSite.Enties
{
    public class BaseDbModel
    {       
        public bool IsDeleted { get; set; }
       
        public string CreateUser { get; set; }
     
        public DateTime CreateTime { get; set; }
 
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string UpdateUser { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime? UpdateTime { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public bool? IsInitData { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public int SortId { get; set; }
    }
}
