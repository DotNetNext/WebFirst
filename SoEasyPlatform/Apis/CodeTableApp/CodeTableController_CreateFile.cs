using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoEasyPlatform.Apis
{
    /// <summary>
    /// 创建文件接口相关业务
    /// </summary>
    public partial class CodeTableController : BaseController
    {
        private List<EntitiesGen> GetGenList(List<CodeTable> tableList, List<CodeType> types)
        {
            List<EntitiesGen> result = new List<EntitiesGen>();
            foreach (var item in tableList)
            {
                EntitiesGen gen = new EntitiesGen()
                {
                    ClassName = item.ClassName,
                    Description = item.Description,
                    TableName = item.TableName,
                    PropertyGens = new List<PropertyGen>()
                };
                foreach (var column in base.CodeColumnsDb.GetList(it => it.CodeTableId == item.Id))
                {
                    PropertyGen proGen = new PropertyGen()
                    {
                        DbColumnName = column.DbColumnName,
                        Description = column.Description,
                        IsIdentity = column.IsIdentity,
                        IsPrimaryKey = column.IsPrimaryKey,
                        PropertyName = column.ClassProperName,
                        Type = types.First(it => it.Name == column.CodeType).CSharepType,
                        IsNullable = column.Required == false
                    };
                    gen.PropertyGens.Add(proGen);
                }
                result.Add(gen);
            }
            return result;
        }
        private string GetFileName(ProjectViewModel project, EntitiesGen item)
        {
            var p = ".";
            project.FileSuffix = project.FileSuffix.TrimStart('.');
            if (project.FileSuffix.Contains("."))
            {
                p = null;
            }
            return FileSugar.MergeUrl(project.Path, item.ClassName + p + project.FileSuffix);
        }

        private string GetFileName(Project project, EntitiesGen item)
        {
            var p = ".";
            project.FileSuffix = project.FileSuffix.TrimStart('.');
            if (project.FileSuffix.Contains("."))
            {
                p = null;
            }
            return FileSugar.MergeUrl(project.Path, item.ClassName + p + project.FileSuffix);
        }
        private string GetNameSpace(string fileModel, string defaultvalue)
        {
            if (!string.IsNullOrEmpty(fileModel))
            {
                dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(fileModel);
                try
                {
                    return obj[0].name;
                }
                catch
                {
                    return defaultvalue;
                }
            }
            else
            {
                return defaultvalue;
            }
        }
    }
}
