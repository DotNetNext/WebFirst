using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar.IOC;
using SqlSugar;
using Newtonsoft.Json.Linq;

namespace SoEasyPlatform 
{
    public class ProjectController_Biz
    {
        public static void CreateProject(int id)
        {
            var project = DbScoped.Sugar.Queryable<Project>().InSingle(id);
            CreateProject(project);

        }

        public static void CreateProject(Project project)
        {
            var ids = project.FileInfo;
            if (!string.IsNullOrEmpty(ids))
            {
                var idsArray = ids.Split(',').ToList();
                dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(project.FileModel);
                if (obj.Count == idsArray.Count)
                {
                    for (int i = 0; i < idsArray.Count(); i++)
                    {
                        var jsonItem = obj[0];
                        var fileId = ids[i].ToString();
                        var context = DbScoped.Sugar.Queryable<FileInfo>().InSingle(fileId).Content;
                    }
                }
                else 
                {
                    throw new Exception("文件填充格式错误");
                }
            }
        }
    }
}
