using Newtonsoft.Json.Linq;
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
            try
            {
                db.BeginTran();
                foreach (var project in projectPathList)
                {
                    var configUrl = FileSugar.MergeUrl(project, "Config.json");
                    CheckProjectConfig(configUrl);
                    var json = FileSugar.FileToString(configUrl);
                    foreach (var item in JArray.Parse(json))
                    {
                        CheckConfigItemChilds(configUrl, item);
                        var 模版 = item["模版"].ToString();
                        var 文件夹 = item["文件夹"].ToString();
                        var 子目录 = item["子目录"].ToString();
                        var 文件后缀 = item["文件后缀"].ToString();
                        var 描述 = item["描述"].ToString();

                    }

                }
                db.CommitTran();
            }
            catch (System.Exception ex)
            {
                db.RollbackTran();
                throw new System.Exception(ex.Message);
            }
        }

        private void CheckConfigItemChilds(string configUrl, JToken item)
        {
            CheckJsonItem(item["模版"], "模版", item, configUrl);
            CheckJsonItem(item["文件夹"], "文件夹", item, configUrl);
            CheckJsonItem(item["子目录"], "子目录", item, configUrl);
            CheckJsonItem(item["文件后缀"], "文件后缀", item, configUrl);
            CheckJsonItem(item["描述"], "描述", item, configUrl);
        }

        private void CheckJsonItem(JToken jToken, string keyName, JToken item,string url)
        {
            if (jToken == null) 
            {
                throw new System.Exception(url+"配置错误 ,缺少: "+keyName+"节点 。"+item.ToString() );
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
