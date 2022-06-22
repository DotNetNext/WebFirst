using SqlSugar;
using System.IO;
using System.Xml.Linq;

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
            foreach (var project in projectPathList)
            {
                var configUrl = FileSugar.MergeUrl(project, "Config.json");
                CheckProjectConfig(configUrl);
                var json = XElement.Parse(configUrl);
            }
        }

        /// <summary>
        /// 验证是否存在项目配置
        /// </summary>
        /// <param name="configUrl"></param>
        /// <exception cref="System.Exception"></exception>
        private static void CheckProjectConfig(string configUrl)
        {
            if (FileSugar.IsExistFile(configUrl) == false)
            {
                throw new System.Exception("没有找到文件" + configUrl);
            }
        }
    }
}
