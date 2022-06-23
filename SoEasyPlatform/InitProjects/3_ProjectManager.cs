using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace SoEasyPlatform
{
    /// <summary>
    /// 方案管理
    /// </summary>
    public partial class InitTable
    {
        private string projectNames;
        private List<int> AddProjects(string sln, string slnName)
        {
            List<int> result = new List<int>();
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
                var tempId = AddTemplate(configUrl, 文件夹, 描述, tempUrl, slnName);
                var projectPath = FileSugar.MergeUrl(sln, 文件夹);
                var id = AddProject(tempId, slnName, 文件夹, sln, projectPath, 文件后缀);
                result.Add(id);
            }
            return result;
        }

        private int AddProject(int tempId,string slnName, string apiName, string rootPath, string projectPath, string suff)
        {
            var files = Directory.GetFiles(projectPath);
            Project project = new Project();
            List<int> fieldIds = new List<int>();
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
                    SolutionId = groupId + "",
                    Sort = 999,

                };
                var fieldId = db.Insertable(file).ExecuteReturnIdentity();
                fieldIds.Add(fieldId);
            }
            project.ProjentName = slnName+"_"+apiName;
            projectNames += (apiName + ",");
            project.Path = projectPath;
            project.SolutionId = "0";
            project.TemplateId1 = tempId + "";
            project.FileInfo = String.Join(",", fieldIds);
            project.FileSuffix = suff;
            project.SolutionId = groupId + "";
            //project.ty = tempTypeId + "";
            var pid = db.Insertable(project).ExecuteReturnIdentity();
            return pid;
        }
    }
}
