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
        /// <summary>
        /// 打开目录
        /// </summary>
        /// <param name="disOpen"></param>
        /// <param name="project"></param>
        private static void OpenPath(bool disOpen, Project project)
        {
            if (disOpen)
            {
                Task.Run(() =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start("explorer.exe",new System.IO.DirectoryInfo(project.Path).Parent.FullName);
                    }
                    catch
                    {

                    }
                });
            }
        }
        private List<EntitiesGen> GetGenList(List<CodeTable> tableList, List<CodeType> types,SqlSugar.DbType databasedbType)
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
                    var codeType = types.First(it => it.Name == column.CodeType);
                    if (codeType.Id == 3)
                    {
                        PropertyGen proGen = new PropertyGen()
                        {
                            DbColumnName = column.DbColumnName,
                            Description = column.Description,
                            IsIdentity = column.IsIdentity,
                            IsPrimaryKey = column.IsPrimaryKey,
                            PropertyName = GetPropertyName(column.ClassProperName),
                            Type = GetType(column),
                            IsNullable = column.Required == false,
                            DbType ="",
                            Length = 0,
                            DecimalDigits = 0,
                            IsIgnore = true,
                            CodeType=column.CodeType
                        };
                        gen.PropertyGens.Add(proGen);
                    }
                    else
                    {
                        var dbType = GetTypeInfoByDatabaseType(codeType.DbType, databasedbType);
                        PropertyGen proGen = new PropertyGen()
                        {
                            DbColumnName = column.DbColumnName,
                            Description = column.Description,
                            IsIdentity = column.IsIdentity,
                            IsPrimaryKey = column.IsPrimaryKey,
                            PropertyName = GetPropertyName(column.ClassProperName),
                            Type = IsSpecialType(column) ? GetType(column) : codeType.CSharepType,
                            IsNullable = column.Required == false,
                            DbType = dbType.Name,
                            Length = dbType.Length,
                            DecimalDigits = dbType.DecimalDigits,
                            IsSpecialType= IsSpecialType(column),
                            CodeType= column.CodeType
                        };
                        gen.PropertyGens.Add(proGen);
                    }
                }
                result.Add(gen);
            }
            return result;
        }

        private static bool IsSpecialType(CodeColumns column)
        {
            return Regex.IsMatch(column.ClassProperName, @"\[.+\]");
        }

        private static string GetType(CodeColumns column)
        {
            string type = "string";
            if (IsSpecialType(column))
            {
                type = Regex.Match(column.ClassProperName, @"\[(.+)\]").Groups[1].Value;
            }

            return type;
        }

        private static string GetPropertyName(string name)
        {
            return Regex.Replace(name, @"\[(.+)\]", "");
        }

        private DbTypeInfo GetTypeInfoByDatabaseType(DbTypeInfo[] dbType, DbType databasedbType)
        {
            DbTypeInfo result = dbType.First();
            List<string> mstypes = new List<string>();
            switch (databasedbType)
            {
                case DbType.MySql:
                    mstypes = SqlSugar.MySqlDbBind.MappingTypesConst.Select(it => it.Key.ToLower()).ToList();
                    break;
                case DbType.SqlServer:
                    mstypes= SqlSugar.SqlServerDbBind.MappingTypesConst.Select(it => it.Key.ToLower()).ToList();
                    break;
                case DbType.Sqlite:
                    mstypes = SqlSugar.SqliteDbBind.MappingTypesConst.Select(it => it.Key.ToLower()).ToList();
                    break;
                case DbType.Oracle:
                    mstypes = SqlSugar.OracleDbBind.MappingTypesConst.Select(it => it.Key.ToLower()).ToList();
                    break;
                case DbType.PostgreSQL:
                    mstypes = SqlSugar.PostgreSQLDbBind.MappingTypesConst.Select(it => it.Key.ToLower()).ToList();
                    break;
                case DbType.Dm:
                    mstypes = SqlSugar.DmDbBind.MappingTypesConst.Select(it => it.Key.ToLower()).ToList();
                    break;
                case DbType.Kdbndp:
                    mstypes = SqlSugar.KdbndpDbBind.MappingTypesConst.Select(it => it.Key.ToLower()).ToList();
                    break;
                default:
                    break;
            }
            result = dbType.FirstOrDefault(it => mstypes.Contains(it.Name.ToLower()));
            if (result == null) 
            {
                throw new Exception("WebFirst暂时不支持类型" + string.Join(",",dbType.Select(it => it.Name)));
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
            string name = item.ClassName;
            project.FileSuffix = project.FileSuffix.TrimStart('.');
            if (project.FileSuffix.Contains("."))
            {
                p = null;
            }
            if (!string.IsNullOrEmpty(project.NameFormat)) 
            {
                name = string.Format(project.NameFormat, item.ClassName);
            }
            return FileSugar.MergeUrl(project.Path, name + p + project.FileSuffix);
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
