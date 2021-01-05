using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CodeTableController : BaseController
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
            var list = NugetDb.AsSugarClient().Queryable<CodeTable, Database>(
                 (it, db) => new JoinQueryInfos(
                       JoinType.Left, it.DbId == db.Id
                     )
                )
                .Where(it => it.IsDeleted == false)
                .Where(it=>it.DbId==model.DbId)
                .WhereIF(!string.IsNullOrEmpty(model.ClassName), it => it.ClassName.Contains(model.ClassName) || it.TableName.Contains(model.ClassName))
                .OrderBy(it => it.Id)
                .Select((it, db) => new CodeTableGridViewModel()
                {
                    Id = SqlFunc.GetSelfAndAutoFill(it.Id),
                    DbName = db.Desc
                })
                .ToPageList(model.PageIndex, model.PageSize, ref count);
            result.Data.Rows = list;
            result.Data.Total = count;
            result.Data.PageSize = model.PageSize;
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
        public ActionResult<ApiResult<CodeTableViewModel>> GetCodeTableInfo([FromForm]string id)
        {
            var result = new ApiResult<CodeTableViewModel>();
            result.Data= mapper.Map< CodeTableViewModel >(base.CodeTableDb.GetById(id));
            result.Data.ColumnInfoList = mapper.Map<List<CodeColumnsViewModel>>(base.CodeColumnsDb.GetList(it=>it.CodeTableId==Convert.ToInt32(id)));
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
        public ActionResult<ApiResult<bool>> SaveCodeTable([FromForm]  string model)
        {
            var result = new ApiResult<bool>();
            CodeTableViewModel viewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CodeTableViewModel>(model);
            base.Check(string.IsNullOrEmpty(viewModel.TableName) || string.IsNullOrEmpty(viewModel.ClassName), "表名或者实体类名必须填一个");
            viewModel.ColumnInfoList = viewModel.ColumnInfoList
                .Where(it => !string.IsNullOrEmpty(it.ClassProperName) || !string.IsNullOrEmpty(it.DbColumnName)).ToList();
            base.Check(viewModel.ColumnInfoList.Count == 0, "请配置实体属性");
            var dbTable = mapper.Map<CodeTable>(viewModel);
            CodeTableWaste.AutoFillTable(dbTable);
            var dbColumns = mapper.Map<List<CodeColumns>>(viewModel.ColumnInfoList);
            CodeTableWaste.AutoFillColumns(dbColumns);
            if (viewModel.Id ==null||viewModel.Id == 0)
            {
                CodeTableWaste.CheckAddName(viewModel, CodeTableDb);
                var id=CodeTableDb.InsertReturnIdentity(dbTable);
                foreach (var item in dbColumns)
                {
                    item.CodeTableId = id;
                }
                CodeColumnsDb.InsertRange(dbColumns);
            }
            else 
            {
                CodeTableWaste.CheckUpdateName(viewModel, CodeTableDb);
                CodeTableDb.Update(dbTable);
                foreach (var item in dbColumns)
                {
                    item.CodeTableId = dbTable.Id;
                }

                var oldIds = CodeColumnsDb.GetList(it => it.CodeTableId == dbTable.Id).Select(it => it.Id).ToList();
                var delIds = oldIds.Where(it => !dbColumns.Select(y => y.Id).Contains(it)).ToList();
                CodeColumnsDb.DeleteByIds(delIds.Select(it => (object)it).ToArray());

                var updateColumns = dbColumns.Where(it => it.Id > 0).ToList();
                if (updateColumns.Count > 0)
                {
                    CodeColumnsDb.UpdateRange(updateColumns);
                }

                var insertColumns = dbColumns.Where(it => it.Id == 0).ToList();
                if (insertColumns.Count > 0)
                {
                    CodeColumnsDb.InsertRange(insertColumns);
                }
              
            }
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
                result.Data=model.DbType + "格式不正确";
                return result;
            }

            CodeTypeDb.Insert(codetype);
            result.IsSuccess = true;
            result.Data = Pubconst.MESSAGEADDSUCCESS;
            return result;
        }
        #endregion

    }
}
