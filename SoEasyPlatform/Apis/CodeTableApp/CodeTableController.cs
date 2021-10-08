using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SqlSugar;

namespace SoEasyPlatform.Apis
{
    /// <summary>
    /// 虚拟类配置
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public partial class CodeTableController : BaseController
    {
        IMapper _mapper;
        public CodeTableController(IMapper mapper) : base(mapper)
        {
            _mapper = mapper;
        }

        #region CodeTable CRUD
        /// <summary>
        /// 生成表根据视图
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [HttpPost]
        [ExceptionFilter]
        [Route("CreateTableByView")]
        public ActionResult<ApiResult<string>> CreateTableByView([FromForm] string ViewSql, [FromForm] int dbid, [FromForm] string className)
        {
            ApiResult<string> result = new ApiResult<string>() { IsSuccess = true };
            var tableDb = base.GetTryDb(dbid);
            var dt = tableDb.Ado.GetDataTable(ViewSql);
            base.Check(dt.Rows.Count == 0, "SQL查询必须要有一条记录才能创建类");
            CodeTable table = new CodeTable()
            {
                TableName = className,
                CreateTime = DateTime.Now,
                ClassName = className,
                IsLock = true,
                DbId = dbid,
                UpdateTime = DateTime.Now,
                Description = "数据源导入"
            };
            var id = Db.Insertable(table).ExecuteReturnIdentity();
            List<CodeColumns> cols = new List<CodeColumns>();
            var listtypes = CodeTypeDb.GetList();
            foreach (System.Data.DataColumn item in dt.Columns)
            {
                CodeColumns columns = new CodeColumns()
                {
                    CodeTableId = id,
                    ClassProperName = item.ColumnName,
                    DbColumnName = item.ColumnName,
                    CodeType = listtypes.FirstOrDefault(it => it.CSharepType.Equals(item.DataType.Name, StringComparison.OrdinalIgnoreCase) || it.DbType.Any(y => y.Name.Equals(item.DataType.Name, StringComparison.OrdinalIgnoreCase)))?.Name
                };
                if (item.DataType.Name.ToLower() == "int32")
                {
                    columns.CodeType = listtypes.Where(it => it.CSharepType == "int").First().Name;
                }
                if (item.DataType.Name.ToLower() == "int16")
                {
                    columns.CodeType = listtypes.Where(it => it.CSharepType == "short").First().Name;
                }
                if (item.DataType.Name.ToLower() == "int64")
                {
                    columns.CodeType = listtypes.Where(it => it.CSharepType == "long").First().Name;
                }
                if (string.IsNullOrEmpty(columns.CodeType))
                {
                    columns.CodeType = listtypes.Where(it => it.CSharepType == "string").First().Name;
                }
                cols.Add(columns);
            }
            Db.Insertable(cols).ExecuteReturnIdentity();
            result.IsSuccess = true;
            result.Data = result.Message = "创建成功";
            return result;
        }
        /// <summary>
        /// 获取虚拟类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCodeTableList")]
        public ActionResult<ApiResult<TableModel<CodeTableGridViewModel>>> GetCodeTableList([FromForm] CodeTableViewModel model)
        {
            var result = new ApiResult<TableModel<CodeTableGridViewModel>>();
            result.Data = new TableModel<CodeTableGridViewModel>();
            int count = 0;
            var list = Db.Queryable<CodeTable, Database>(
                 (it, db) => new JoinQueryInfos(
                       JoinType.Left, it.DbId == db.Id
                     )
                )
                .Where(it => it.DbId == model.DbId)
                .WhereIF(!string.IsNullOrEmpty(model.ClassName), it => it.ClassName.Contains(model.ClassName) || it.TableName.Contains(model.ClassName))
                .OrderBy(it => it.TableName)
                .Select((it, db) => new CodeTableGridViewModel()
                {
                    Id = it.Id.SelectAll(),
                    DbName = db.Desc
                })
                .ToPageList(model.PageIndex, int.MaxValue, ref count)
                .OrderBy(it => it.CreateTime.AddSeconds(100) >= DateTime.Now ? 0 : 1).ToList();
            result.Data.Rows = list;
            result.Data.Total = count;
            result.Data.PageSize = count;
            result.Data.PageNumber = model.PageIndex;
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        /// 获取虚拟类根据主键
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCodeTableInfo")]
        public ActionResult<ApiResult<CodeTableViewModel>> GetCodeTableInfo([FromForm] string id)
        {
            var result = new ApiResult<CodeTableViewModel>();
            result.Data = mapper.Map<CodeTableViewModel>(base.CodeTableDb.GetById(id));
            result.Data.ColumnInfoList = mapper.Map<List<CodeColumnsViewModel>>(base.CodeColumnsDb.GetList(it => it.CodeTableId == Convert.ToInt32(id)));
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 保存虚拟类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [ExceptionFilter]
        [Route("SaveCodeTable")]
        public ActionResult<ApiResult<bool>> SaveCodeTable([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            CodeTableViewModel viewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CodeTableViewModel>(model);
            viewModel.CreateTime = DateTime.Now;
            SaveCodeTableToDb(viewModel);
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        /// 删除虚拟类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteCodeTable")]
        public ActionResult<ApiResult<bool>> DeleteCodeTable([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CodeTableViewModel>>(model);
                Db.Utilities.PageEach(list, 50, eitem =>
                {
                    var exp = Expressionable.Create<CodeTable>();
                    foreach (var item in eitem)
                    {
                        exp.Or(it => it.Id == item.Id);
                    }
                    CodeTableDb.Update(it => new CodeTable() { IsDeleted = true }, exp.ToExpression());
                });
            }
            result.IsSuccess = true;
            return result;
        }

        #endregion

        #region Code Type CRUD

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCodeTypeList")]
        public ActionResult<ApiResult<TableModel<CodeTypeGridViewModel>>> GetCodeTypeList([FromForm] CodeTypeViewModel model)
        {
            model.PageSize = 20;
            var result = new ApiResult<TableModel<CodeTypeGridViewModel>>();
            result.Data = new TableModel<CodeTypeGridViewModel>();
            int count = 0;
            var list = CodeTypeDb.AsSugarClient().Queryable<CodeType>()
                .WhereIF(!string.IsNullOrEmpty(model.Name), it => it.Name.Contains(model.Name) || it.CSharepType.Contains(model.Name))
                .OrderBy(it => it.Sort)
                .OrderBy(it => it.Id)
                .ToPageList(model.PageIndex, model.PageSize, ref count);
            var codeGridList = mapper.Map<List<CodeTypeGridViewModel>>(list);
            foreach (var item in codeGridList)
            {
                var dbType = list.First(it => it.Id == item.Id).DbType;
                item.DbType = Newtonsoft.Json.JsonConvert.SerializeObject(dbType);
            }
            result.Data.Rows = codeGridList;
            result.Data.Total = count;
            result.Data.PageSize = model.PageSize;
            result.Data.PageNumber = model.PageIndex;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 添加类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [Route("SaveCodeType")]
        public ActionResult<ApiResult<string>> SaveCodeType([FromForm] CodeTypeViewModel model)
        {
            var result = new ApiResult<string>();
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            CodeType codetype = new CodeType()
            {
                Id = 0,
                CSharepType = model.CSharepType,
                Name = model.Name,
                Sort = model.Sort.Value
            };
            try
            {
                codetype.DbType = Newtonsoft.Json.JsonConvert.DeserializeObject<DbTypeInfo[]>(model.DbType);
            }
            catch
            {
                result.IsSuccess = false;
                result.Data = model.DbType + "格式不正确";
                return result;
            }

            CodeTypeDb.Insert(codetype);
            result.IsSuccess = true;
            result.Data = Pubconst.MESSAGEADDSUCCESS;
            return result;
        }
        #endregion

        #region Create File
        /// <summary>
        /// 生成实体
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [ExceptionFilter]
        [Route("createfile")]
        public ActionResult<ApiResult<bool>> CreateFile([FromForm] ProjectViewModel model)
        {
            base.Check(model.Reference != null && model.Reference.Split(',').Contains(model.Id + ""), "方案不能自已引用自已");
            base.Check(new System.IO.DirectoryInfo(model.Path)?.Parent?.Parent == null, "路径" + model.Path + "错误，正确格式  C:\\解决方案\\项目");
            var result = new ApiResult<bool>();
            var tempInfo = TemplateDb.GetById(model.TemplateId1);
            model.ModelId = tempInfo.TemplateTypeId;
            var dbModel = mapper.Map<Project>(model);
            var s = base.Db.Storageable(dbModel)
                .SplitError(it => string.IsNullOrEmpty(model.Tables), "请选择表")
                .SplitError(it => Db.Queryable<Project>().Any(s => s.Id != model.Id && s.ProjentName == model.ProjentName && s.TemplateId1 == model.TemplateId1), "方前方案已存在请换个名字或者使用方案生成")
                .Saveable()
                .ToStorage();
            var id = s.AsInsertable.ExecuteReturnIdentity();
            s.AsUpdateable.ExecuteCommand();
            if (s.ErrorList.Any())
            {
                throw new Exception(s.ErrorList.First().StorageMessage);
            }
            //var template = TemplateDb.GetById(model.TemplateId1).Content;
            //var tableids = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CodeTypeGridViewModel>>(model.Tables).Select(it => it.Id).ToList();
            //var tableList = CodeTableDb.GetList(it => tableids.Contains(it.Id));
            //int dbId = tableList.First().DbId;
            //var connection = base.GetTryDb(dbId);
            //List<EntitiesGen> genList = GetGenList(tableList, CodeTypeDb.GetList(), connection.CurrentConnectionConfig.DbType);
            //string key = TemplateHelper.EntityKey + template.GetHashCode();
            //foreach (var item in genList)
            //{
            //    item.name_space = GetNameSpace(model.FileModel,item.name_space);
            //    var html = TemplateHelper.GetTemplateValue(key, template, item);
            //    var fileName = GetFileName(model, item);
            //    FileSugar.CreateFileReplace(fileName, html, Encoding.UTF8);
            //}
            //ProjectController_Common.CreateProject(dbModel);
            result.IsSuccess = true;
            result.Message = model.Id == 0 ? "创建成功" : "修改成功";
            return result;
        }

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [ExceptionFilter]
        [Route("CreateFileByProjectId")]
        public ActionResult<ApiResult<bool>> CreateFileByProjectId([FromForm] ProjectViewModel2 model, bool disOpen = true)
        {
            var result = new ApiResult<bool>();
            var tables = model.Tables;
            var project = ProjectDb.GetSingle(it => it.Id == model.ProjectId);
            base.Check(project == null, "请选择方案");
            model.Tables = tables;
            var template = TemplateDb.GetById(project.TemplateId1).Content;
            var tableids = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CodeTypeGridViewModel>>(model.Tables).Select(it => it.Id).ToList();
            var tableList = CodeTableDb.GetList(it => tableids.Contains(it.Id));
            int dbId = tableList.First().DbId;
            var connection = base.GetTryDb(dbId);
            List<EntitiesGen> genList = GetGenList(tableList, CodeTypeDb.GetList(), connection.CurrentConnectionConfig.DbType);
            string key = TemplateHelper.EntityKey + template.GetHashCode();
            foreach (var item in genList)
            {
                item.name_space = GetNameSpace(project.FileModel, item.name_space);
                var html = TemplateHelper.GetTemplateValue(key, template, item);
                var fileName = GetFileName(project, item);
                FileSugar.CreateFileReplace(fileName, html, Encoding.UTF8);
            }
            OpenPath(disOpen, project);
            ProjectController_Common.CreateProject(project.Id, model.DbId.Value);
            result.IsSuccess = true;
            result.Message = "生成生功";
            return result;
        }
        #endregion

        #region Export
        /// <summary>
        ////导出文档
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ExceptionFilter]
        [Route("exportfile")]
        public ApiResult<bool> Exportfile([FromForm] string model, [FromForm] int dbid)
        {
            ApiResult<bool> result = new ApiResult<bool>() { IsSuccess = true };
            var tableDb = base.GetTryDb(dbid);
            var dts = Export(model, tableDb);
            var bytes = Table_ToExcel.ExportExcel(dts, "数据库文档.xlsx");
            var url = FileSugar.MergeUrl(Startup.GetCurrentDirectory(), "excel/数据库文档" + SqlSugar.SnowFlakeSingle.Instance.getID() + ".xlsx");
            FileSugar.CreateFile(url, bytes);
            OpenPath(url);
            return result;
        }
        #endregion

        #region CreateTable
        /// <summary>
        ////生成表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ExceptionFilter]
        [Route("CreateTables")]
        public ActionResult<ApiResult<bool>> CreateTables([FromForm] string model, [FromForm] int dbid)
        {
            var tableDb = base.GetTryDb(dbid);
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CodeTableViewModel>>(model);
                var oldList = CodeTableDb.AsQueryable().In(list.Select(it => it.Id).ToList()).ToList();
                base.Check(oldList.Any(it => it.IsLock), string.Join(",", oldList.Where(it => it.IsLock).Select(it => it.ClassName)) + "是锁表状态禁止建表");
                List<EntitiesGen> genList = GetGenList(oldList, CodeTypeDb.GetList(), tableDb.CurrentConnectionConfig.DbType);
                foreach (var item in genList)
                {
                    item.PropertyGens = item.PropertyGens.Where(it => it.IsIgnore == false).ToList();
                    foreach (var property in item.PropertyGens)
                    {
                        if (property.IsSpecialType)
                        {
                            property.Type = "string";
                        }
                    }
                }
                string key = TemplateHelper.EntityKey + SyntaxTreeHelper.TemplateString.GetHashCode();
                foreach (var item in genList)
                {
                    var classString = TemplateHelper.GetTemplateValue(key, SyntaxTreeHelper.TemplateString, item);
                    var type = SyntaxTreeHelper.GetModelTypeByClass(classString, item.ClassName);
                    tableDb.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices() {
                        EntityNameService = (type, info) =>
                        {
                            if (info.EntityName == item.ClassName || (info.EntityName == null && info.DbTableName == item.ClassName))
                            {
                                info.EntityName = item.ClassName;
                                info.DbTableName = item.TableName;
                                info.TableDescription = item.Description;
                            }
                        },
                        EntityService = (type, info) =>
                          {
                              if (info.EntityName == item.ClassName)
                              {
                                  var column = item.PropertyGens.FirstOrDefault(it => it.PropertyName == info.PropertyName);
                                  info.DbColumnName = column.DbColumnName;
                                  info.ColumnDescription = column.Description;
                                  info.IsNullable = column.IsNullable;
                                  info.Length = Convert.ToInt32(column.Length);
                                  info.DecimalDigits = Convert.ToInt32(column.DecimalDigits);
                                  info.IsPrimarykey = column.IsPrimaryKey;
                                  info.IsIdentity = column.IsIdentity;
                                  info.IsIgnore = column.IsIgnore;
                                  info.DataType = column.DbType;
                                  if (tableDb.CurrentConnectionConfig.DbType == DbType.Sqlite && info.IsIdentity)
                                  {
                                      info.DataType = "integer";
                                  }
                              }
                          }
                    };
                    tableDb.CodeFirst.InitTables(type);
                }

            }
            result.IsSuccess = true;
            return result;
        }

        #endregion

        #region  Update entity by db
        /// <summary>
        /// 从数据库导入虚拟类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [ExceptionFilter]
        [Route("savecodetableimport")]
        public ActionResult<ApiResult<bool>> SaveCodetableImport([FromForm] int dbid, [FromForm] string model)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DbTableGridViewModel>>(model);
            var tableDb = base.GetTryDb(dbid);
            var systemDb = Db;
            var type = CodeTypeDb.GetList();
            var entityList = CodeTableDb.GetList(it => it.DbId == dbid);
            systemDb.BeginTran();
            try
            {
                List<CodeTable> Inserts = new List<CodeTable>();
                foreach (var item in list)
                {
                    CodeTableViewModel code = new CodeTableViewModel()
                    {
                        ClassName = PubMehtod.GetCsharpName(item.Name),
                        TableName = item.Name,
                        DbId = dbid,
                        Description = item.Description,
                        ColumnInfoList = new List<CodeColumnsViewModel>()
                    };
                    var entity = entityList.FirstOrDefault(it => it.TableName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                    if (entity == null)
                    {
                        entity = new CodeTable();
                    }
                    foreach (var columnInfo in tableDb.DbMaintenance.GetColumnInfosByTableName(item.Name, false))
                    {
                        var typeInfo = GetEntityType(type, columnInfo, this, tableDb.CurrentConnectionConfig.DbType);
                        CodeColumnsViewModel column = new CodeColumnsViewModel()
                        {
                            ClassProperName = PubMehtod.GetCsharpName(columnInfo.DbColumnName),
                            DbColumnName = columnInfo.DbColumnName,
                            Description = columnInfo.ColumnDescription,
                            IsIdentity = columnInfo.IsIdentity,
                            IsPrimaryKey = columnInfo.IsPrimarykey,
                            Required = columnInfo.IsNullable == false,
                            CodeTableId = entity.Id,
                            CodeType = typeInfo.CodeType.Name,
                            Length = typeInfo.DbTypeInfo.Length,
                            DecimalDigits = typeInfo.DbTypeInfo.DecimalDigits
                        };
                        code.ColumnInfoList.Add(column);
                    }
                    SaveCodeTableToDb(code);
                };
                systemDb.CommitTran();
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                systemDb.RollbackTran();
                throw ex;
            }
            return result;
        }

        /// <summary>
        ////根据数据库更新虚拟类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ExceptionFilter]
        [Route("UpdateEntity")]
        public ActionResult<ApiResult<bool>> UpdateEntity([FromForm] string model, [FromForm] int dbid)
        {
            var tableDb = base.GetTryDb(dbid);
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CodeTableViewModel>>(model);
                Db.Utilities.PageEach(list, 20, pageList =>
                {
                      UpdateEntityItem(dbid, tableDb, pageList);
                  });
            }
            result.IsSuccess = true;
            return result;
        }

        private void UpdateEntityItem(int dbid, SqlSugarClient tableDb, List<CodeTableViewModel> list)
        {
            var oldList = CodeTableDb.AsQueryable().In(list.Select(it => it.Id).ToList()).ToList();
            var oldColumns = Db.Queryable<CodeColumns, CodeTable>((c, t) => c.CodeTableId == t.Id).Where((c, t) => oldList.Select(it => it.Id).Contains(t.Id)).Select((c, t) => new { TableId = t.Id, TableName = t.TableName, Columns = c }).ToList();
            var alltables = tableDb.DbMaintenance.GetTableInfoList(false).Select(it => it.Name.ToLower()).ToList();
            var ids = list.Select(it => it.Id).ToList();
            var tableNames = list.Select(it => it.TableName.ToLower()).ToList();
            var errorTables = list.Where(it => !alltables.Contains(it.TableName.ToLower()) && !alltables.Contains(it.ClassName.ToLower())).ToList();
            base.Check(errorTables.Any(), string.Join(",", errorTables.Select(y => y.TableName ?? y.ClassName)) + "未创建表不能同步");
            try
            {
                Db.BeginTran();
                CodeTableDb.DeleteByIds(ids.Select(it => (object)it).ToArray());
                var dbTableGridList = tableDb.DbMaintenance.GetTableInfoList(false).Where(it => tableNames.Contains(it.Name.ToLower())).Select(it => new DbTableGridViewModel()
                {
                    Description = it.Description,
                    Name = it.Name
                });
                if (dbTableGridList.Any())
                {
                    SaveCodetableImport(dbid, Newtonsoft.Json.JsonConvert.SerializeObject(dbTableGridList));
                }
                foreach (var item in oldList)
                {
                    CodeTableDb.AsUpdateable(item).UpdateColumns(it => it.ClassName).WhereColumns(it => it.TableName).ExecuteCommand();
                }
                List<CodeColumns> UpdateColumns = new List<CodeColumns>();
                foreach (var item in oldColumns.GroupBy(it => new { it.TableId, it.TableName }).ToList())
                {
                    var tableId = CodeTableDb.AsQueryable().Where(it => it.TableName == item.Key.TableName && it.DbId == dbid).First()?.Id;
                    if (tableId != null)
                    {
                        var columns = CodeColumnsDb.AsQueryable().Where(it => it.CodeTableId == tableId).ToList();
                        foreach (var col in columns)
                        {
                            var addColumn = item.FirstOrDefault(it => it.Columns.DbColumnName == col.DbColumnName);
                            if (addColumn != null)
                            {
                                col.ClassProperName = addColumn.Columns.ClassProperName;
                                UpdateColumns.Add(col);
                            }
                            else
                            {

                            }
                        }
                        foreach (var oldItem in item.ToList())
                        {
                            if (oldItem.Columns.CodeType.Equals("ignore", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var mapp = _mapper.Map<CodeColumns>(oldItem.Columns);
                                mapp.CodeTableId = columns[0].CodeTableId;
                                CodeColumnsDb.AsInsertable(mapp).ExecuteCommand();
                            }
                        }
                    }
                }
                CodeColumnsDb.AsUpdateable(UpdateColumns).UpdateColumns(it => it.ClassProperName).ExecuteCommand();
                Db.CommitTran();
            }
            catch (Exception ex)
            {
                Db.RollbackTran();
                throw ex;
            }
        }


        #endregion

        #region Copy
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [ExceptionFilter]
        [Route("Copy")]
        public ActionResult<ApiResult<string>> Copy([FromForm] ProjectViewModel2 model)
        {
            var result = new ApiResult<string>();
            var tables = model.Tables;
            var project = ProjectDb.GetSingle(it => it.Id == model.ProjectId);
            base.Check(project == null, "请选择方案，没有方案可以在手动生成里面创建");
            model.Tables = tables;
            var template = TemplateDb.GetById(project.TemplateId1).Content;
            var tableids = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CodeTypeGridViewModel>>(model.Tables).Select(it => it.Id).ToList();
            var tableList = CodeTableDb.GetList(it => tableids.Contains(it.Id));
            int dbId = tableList.First().DbId;
            var connection = base.GetTryDb(dbId);
            List<EntitiesGen> genList = GetGenList(tableList, CodeTypeDb.GetList(), connection.CurrentConnectionConfig.DbType);
            string key = TemplateHelper.EntityKey + template.GetHashCode();
            foreach (var item in genList.Take(1))
            {
                item.name_space = GetNameSpace(project.FileModel, item.name_space);
                result.Data = TemplateHelper.GetTemplateValue(key, template, item);
            }
            ProjectController_Common.CreateProject(project.Id, model.DbId.Value);
            result.IsSuccess = true;
            return result;
        }
        #endregion

        #region Append Field
        /// <summary>
        ///  追加字段
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [ExceptionFilter]
        [Route("SaveCommField")]
        public ActionResult<ApiResult<string>> SaveCommField([FromForm] string Field,[FromForm] string model,[FromForm] int? dbid)
        {
            ApiResult<string> result = new ApiResult<string>();
            var fields = (Field+"").Split(',').Where(it=>!string.IsNullOrEmpty(it)).ToList();
            var fieldInfoList = CommonFieldDb.GetList(it => fields.Contains(it.Id.ToString()));
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CodeTableGridViewModel>>(model);
            List<CodeColumns> addcolumns = new List<CodeColumns>();
            foreach (var item in list)
            {
                var columns = CodeColumnsDb.GetList(it => it.CodeTableId == item.Id);
    
                foreach (var filedItem in fieldInfoList)
                {
                    if (!columns.Any(y=>y.DbColumnName.ToLower() == filedItem.DbColumnName.ToLower())&& !columns.Any(y => y.ClassProperName.ToLower() == filedItem.ClassProperName.ToLower()))
                    {
                        if (!addcolumns.Any(it => it.DbColumnName == filedItem.DbColumnName && it.CodeTableId == Convert.ToInt32(item.Id)))
                        {
                            addcolumns.Add(new CodeColumns()
                            {
                                CodeTableId = Convert.ToInt32(item.Id),
                                DbColumnName = filedItem.DbColumnName,
                                ClassProperName = filedItem.ClassProperName,
                                CodeType = CodeTypeDb.GetById(filedItem.CodeType).Name,
                                IsIdentity = filedItem.IsIdentity,
                                IsPrimaryKey = filedItem.IsPrimaryKey,
                                Required = filedItem.Required,

                            });
                        }
                    }
                }
            }
            CodeColumnsDb.InsertRange(addcolumns);
            result.IsSuccess = true;
            result.Data = "追加成功";
            return result;
         }

        #endregion

        #region Append Property
        /// <summary>
        ///  追加属性
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ExceptionFilter]
        [Route("SaveTagProperty")]
        public ActionResult<ApiResult<string>> SaveTagProperty([FromForm] string model, [FromForm] int? dbid = 0)
        {
            dynamic json = JObject.Parse(model);
            string table = json.table.Value ;
            Db.Deleteable<MappingProperty>().Where(it => it.DbId == dbid.Value && it.TableName == table).ExecuteCommand();
            foreach (var item in json.columns)
            {
                var key = item.key.Value;
                var value = item.value;
                if (value.Count > 0)
                {
                    foreach (var tag in value)
                    {

                        Db.Insertable(new MappingProperty() { DbId = dbid.Value, TableName = table, ColumnName = key, TagId= tag.Value }).ExecuteCommand();
                    }
                }
            }
            ApiResult<string> result = new ApiResult<string>() { IsSuccess = true };
            return result;
        } 
        #endregion
    }
}
