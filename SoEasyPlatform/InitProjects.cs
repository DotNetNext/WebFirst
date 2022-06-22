using SqlSugar;
using System.IO;

namespace SoEasyPlatform
{
    /// <summary>
    /// 用来初始化整个项目 /wwwroot/template/Projects
    /// </summary>
    public partial class InitTable
    {
        private void InitProjects(SqlSugarClient db)
        {
            var directory = FileSugar.MergeUrl(Directory.GetCurrentDirectory(), "wwwroot", "template", "Projects");
            var projectPathList=Directory.GetDirectories(directory);
        }
    }
}
