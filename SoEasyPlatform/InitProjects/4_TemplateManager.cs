using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace SoEasyPlatform
{
    /// <summary>
    ///模版管理
    /// </summary>
    public partial class InitTable
    {
        int tempTypeId = 0;
        private int AddTemplate(string configUrl, string 文件夹, string 描述, string tempUrl, string slnName)
        {
            var name = slnName + "_" + 文件夹 + 描述;
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
            tempTypeId = temp.TemplateTypeId;
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
    }
}
