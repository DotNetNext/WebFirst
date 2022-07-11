using SqlSugar;
using System.Collections.Generic;

namespace SoEasyPlatform
{
    public class ProjectGroup
    {
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int Id { get; set; }
        public string SolutionPath { get; set; }
        public string Name { get; set; }
        [SugarColumn(IsJson =true,ColumnDataType ="varchar(500)")]
        public int[] ProjectIds { get; set; }
        public string ProjectNames { get; set; }
        public int Sort { get;  set; }
        [SugarColumn(IsNullable =true)]
        public string Description { get; set; }
    }
}
