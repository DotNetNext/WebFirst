using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace SoEasyPlatform.Code.Apis
{
    /// <summary>
    /// 虚拟类配置
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public partial class CodeTableController : BaseController
    {
        public CodeTableController(IMapper mapper) : base(mapper)
        {
        }

        #region Code Table
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
            var list =Db.Queryable<CodeTable, Database>(
                 (it, db) => new JoinQueryInfos(
                       JoinType.Left, it.DbId == db.Id
                     )
                )
                .Where(it => it.DbId == model.DbId)
                .WhereIF(!string.IsNullOrEmpty(model.ClassName), it => it.ClassName.Contains(model.ClassName) || it.TableName.Contains(model.ClassName))
                .OrderBy(it => it.Id)
                .Select((it, db) => new CodeTableGridViewModel()
                {
                    Id = it.Id.SelectAll(),
                    DbName = db.Desc
                })
                .ToPageList(model.PageIndex, 30, ref count);
            result.Data.Rows = list;
            result.Data.Total = count;
            result.Data.PageSize = 30;
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
            SaveCodeTableToDb(viewModel);
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
                        ClassName =PubMehtod.GetCsharpName(item.Name),
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
                    foreach (var columnInfo in tableDb.DbMaintenance.GetColumnInfosByTableName(item.Name))
                    {
                        CodeColumnsViewModel column = new CodeColumnsViewModel()
                        {
                            ClassProperName = PubMehtod.GetCsharpName(columnInfo.DbColumnName),
                            DbColumnName = columnInfo.DbColumnName,
                            Description = columnInfo.ColumnDescription,
                            IsIdentity = columnInfo.IsIdentity,
                            IsPrimaryKey = columnInfo.IsPrimarykey,
                            Required = columnInfo.IsNullable == false,
                            CodeTableId = entity.Id,
                            CodeType = GetEntityType(type, columnInfo, this)
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
                var exp = Expressionable.Create<CodeTable>();
                foreach (var item in list)
                {
                    exp.Or(it => it.Id == item.Id);
                }
                CodeTableDb.Update(it => new CodeTable() { IsDeleted = true }, exp.ToExpression());
            }
            result.IsSuccess = true;
            return result;
        }

        #endregion

        #region Code Type

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
            var result = new ApiResult<bool>();
            var tempInfo = TemplateDb.GetById(model.TemplateId1);
            model.ModelId = tempInfo.TemplateTypeId;
            var dbModel = mapper.Map<Project>(model);
            var s = base.Db.Storageable(dbModel)
                .SplitInsert(it => !string.IsNullOrEmpty(it.Item.ProjentName))
                .SplitError(it => string.IsNullOrEmpty(model.Tables), "请选择表")
                .SplitError(it => Db.Queryable<Project>().Any(s => s.ProjentName == model.ProjentName && s.TemplateId1 == model.TemplateId1), "方前方案已存在请换个名字或者使用方案生成")
                .SplitInsert(it => it.Item.Id > 0).ToStorage();
             var id=s.AsInsertable.ExecuteReturnIdentity();
             s.AsUpdateable.ExecuteCommand();
            if (s.ErrorList.Any())
            {
                throw new Exception(s.ErrorList.First().StorageMessage);
            }
            var template = TemplateDb.GetById(model.TemplateId1).Content;
            var tableids = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CodeTypeGridViewModel>>(model.Tables).Select(it => it.Id).ToList();
            var tableList = CodeTableDb.GetList(it => tableids.Contains(it.Id));
            List<EntitiesGen> genList = GetGenList(tableList, CodeTypeDb.GetList());
            string key = TemplateHelper.EntityKey + template.GetHashCode();
            foreach (var item in genList)
            {
                item.name_space = GetNameSpace(model.FileModel,item.name_space);
                var html = TemplateHelper.GetTemplateValue(key, template, item);
                var fileName = GetFileName(model, item);
                FileSugar.CreateFileReplace(fileName, html, Encoding.UTF8);
            }
            ProjectController_Biz.CreateProject(dbModel);
            result.IsSuccess = true;
            result.Message = "生成生功";
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
        public ActionResult<ApiResult<bool>> CreateFileByProjectId([FromForm] ProjectViewModel2 model)
        {
            var result = new ApiResult<bool>();
            var tables = model.Tables;
            var project = ProjectDb.GetSingle(it => it.Id == model.ProjectId);
            model.Tables = tables;
            var template = TemplateDb.GetById(project.TemplateId1).Content;
            var tableids = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CodeTypeGridViewModel>>(model.Tables).Select(it => it.Id).ToList();
            var tableList = CodeTableDb.GetList(it => tableids.Contains(it.Id));
            List<EntitiesGen> genList = GetGenList(tableList, CodeTypeDb.GetList());
            string key = TemplateHelper.EntityKey + template.GetHashCode();
            foreach (var item in genList)
            {
                var html = TemplateHelper.GetTemplateValue(key, template, item);
                var fileName = GetFileName(project, item);
                FileSugar.CreateFileReplace(fileName, html, Encoding.UTF8);
            }
            ProjectController_Biz.CreateProject(project.Id);
            result.IsSuccess = true;
            result.Message = "生成生功";
            return result;
        }
        #endregion
    }
}
