using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar.IOC;
using SqlSugar;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace SoEasyPlatform
{
    /// <summary>
    /// 项目公用逻辑
    /// </summary>
    public class ProjectController_Common
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
                        ExpandoObject jsonItem = GetJsonItem(obj[i]);
                        var fileId = ids[i].ToString();
                        var fileInfo = DbScoped.Sugar.Queryable<FileInfo>().InSingle(idsArray[i]);
                        var context = fileInfo.Content;

                        var html = TemplateHelper.GetTemplateValue(context, context, jsonItem);
                        var name = (jsonItem as IDictionary<string, object>)["name"];
                        var fileName = FileSugar.MergeUrl(project.Path, name + "." + fileInfo.Suffix.TrimStart('.'));
                        if (!FileSugar.IsExistFile(fileName))
                            FileSugar.CreateFile(fileName, html);
                    }
                }
                else
                {
                    throw new Exception("文件填充格式错误");
                }
            }
        }
        private static List<ExpandoObject> GetJsonItems(dynamic obj)
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            foreach (var item in (obj as JArray))
            {
                if (item is JObject)
                {
                    result.Add(GetJsonItem(item));
                }
            }
            return result;
        }
        private static ExpandoObject GetJsonItem(dynamic obj)
        {
            Dictionary<string, object> old = JObject.FromObject(obj).ToObject<Dictionary<string, object>>();
            ExpandoObject resultExp = new ExpandoObject();
            var result = ((IDictionary<string, object>)resultExp);
            foreach (var item in old)
            {
                if (item.Value is JObject)
                {
                    result.Add(item.Key, GetJsonItem(item.Value));
                }
                else if (item.Value is JArray)
                {
                    result.Add(item.Key, GetJsonItems(item.Value));
                }
                else
                {
                    result.Add(item.Key, item.Value);
                }
            }
            return resultExp;
        }
    }
}
