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
        /// 保存虚拟类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [Route("SaveCodeTable")]
        public ActionResult<ApiResult<string>> SaveCodeTable([FromForm] CodeTableViewModel model)
        {
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var saveObject = base.mapper.Map<CodeTable>(model);
            var result = new ApiResult<string>();
            if (saveObject.Id == 0)
            {
                saveObject.IsDeleted = false;
                CodeTableDb.Insert(saveObject);
                result.IsSuccess = true;
                result.Data = Pubconst.MESSAGEADDSUCCESS;
            }
            else
            {
                saveObject.IsDeleted = false;
                CodeTableDb.Update(saveObject);
                result.IsSuccess = true;
                result.Data = Pubconst.MESSAGEADDSUCCESS;
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

    }
}
