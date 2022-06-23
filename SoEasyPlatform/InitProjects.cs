using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
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
            //var directory = FileSugar.MergeUrl(Directory.GetCurrentDirectory(), "wwwroot", "template", "Projects");
            //var slnListPathList = Directory.GetDirectories(directory);
            //try
            //{
            //    db.BeginTran();
            //    foreach (var sln in slnListPathList)
            //    {
            //        ProjectGroup projectGroup = new ProjectGroup();
            //        AddProjects(db, sln);
            //    }
            //    db.CommitTran();
            //}
            //catch (System.Exception ex)
            //{
            //    db.RollbackTran();
            //    throw new System.Exception(ex.Message);
            //}
        }

        private List<int> AddProjects(SqlSugarClient db, string sln)
        {
            List<int> result = new List<int>();
            var slnName = System.IO.Path.GetFileName(sln);
            var configUrl = FileSugar.MergeUrl(sln, "Config.json");
            CheckProjectConfig(configUrl);
            var json = FileSugar.FileToString(configUrl);
            var projects = JArray.Parse(json);
            foreach (var item in projects)
            {
                CheckConfigItemChilds(configUrl, item);
                var 模版 = item["模版"].ToString();
                var 文件夹 = item["文件夹"].ToString();
                var 子目录 = item["子目录"].ToString();
                var 文件后缀 = item["文件后缀"].ToString();
                var 描述 = item["描述"].ToString();
                var tempUrl = FileSugar.MergeUrl(sln, 模版);
                CheckProjectConfig(tempUrl);
                var tempId = AddTemplate(db, configUrl, 文件夹, 描述, tempUrl, slnName);
                var projectPath = FileSugar.MergeUrl(sln, 文件夹);
                var id= AddProject(db, tempId, 文件夹, sln, projectPath);
                result.Add(id);
            }
            return result;
        }

        private int AddProject(SqlSugarClient db, int tempId, string apiName, string rootPath, string projectPath)
        {
            var files = Directory.GetFiles(projectPath);

            foreach (var filePath in files)
            {

                var path = filePath.Replace(projectPath, "").TrimStart('\\').TrimStart('/');
                FileInfo file = new FileInfo()
                {
                    ChangeTime = DateTime.Now,
                    Content = FileSugar.FileToString(filePath),
                    IsDeleted = false,
                    IsInit = true,
                    Name = "",
                    Json = "{\"name\":\"" + System.IO.Path.GetFileName(filePath) + "\"}",
                    Suffix = "{\"name\":\"" + System.IO.Path.GetExtension(filePath) + "\"}",
                    SolutionId = "",
                    Sort = 999
                };
            }
            //throw new NotImplementedException();
            return 1;
        }

        private int AddTemplate(SqlSugarClient db, string configUrl, string 文件夹, string 描述, string tempUrl,string slnName )
        {
            var name = slnName+"_" +文件夹 + 描述  ;
            var dbtemp = db.Queryable<Template>().First(it => it.Title == name);
            if (dbtemp != null && !string.IsNullOrEmpty(dbtemp.Content))
            {
                return dbtemp.Id;
            }
            else if (dbtemp != null)
            {
                db.Deleteable<Template>().In(dbtemp.Id).ExecuteCommand();
            }
            var temp = new Template()
            {
                Content = FileSugar.FileToString(tempUrl),
                ChangeTime = DateTime.Now,
                IsDeleted = false,
                Sort = 999,
                TemplateTypeName = 描述,
                Title = name,
                IsInit = true,
                TemplateTypeId = Convert.ToInt32(GetTempTypeId(描述, configUrl))
            };
            var tempId = db.Insertable<Template>(temp).ExecuteReturnIdentity();
            return tempId;
        }

        private string GetTempTypeId(string 描述, string url)
        {
            if (描述 == "实体")
                return "1";
            else if (描述 == "业务")
                return "2";
            else if (描述.ToLower() == "web")
                return "3";
            else
            {
                throw new Exception("描述内容错误: 只能是 实体、业务或者Web。 " + url);
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

        private void CheckJsonItem(JToken jToken, string keyName, JToken item, string url)
        {
            if (jToken == null)
            {
                throw new System.Exception(url + "配置错误 ,缺少: " + keyName + "节点 。" + item.ToString());
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
