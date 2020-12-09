using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace SoEasyPlatform.Code.Apis
{
    /// <summary>
    /// 数据库管理
    /// </summary>
    public class DatabaseController :BaseController
    {

        /// <summary>
        /// 获取系统列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getdatabase")]
        public ActionResult<ApiResult<TableModel<DatabaseGridViewModel>>> GetDatabase([FromForm] IndexViewModel model)
        {
            var result = new ApiResult<TableModel<DatabaseGridViewModel>>();
            result.Data = new TableModel<DatabaseGridViewModel>();
            int count = 0;
            var list = databaseDb.AsQueryable()
                .Where(it => it.IsDeleted == false)
                .WhereIF(!string.IsNullOrEmpty(model.Desc), it => it.Desc.Contains(model.Desc))
                .ToPageList(model.PageIndex, model.PageSize, ref count);
            result.Data.Rows = base.mapper.Map<List<DatabaseGridViewModel>>(list);
            result.Data.Total = count;
            result.Data.PageSize = model.PageSize;
            result.Data.PageNumber = model.PageIndex;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 保存数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [Route("savedatabase")]
        public ActionResult<ApiResult<string>> SaveDatabase([FromForm] IndexViewModel model)
        {
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var saveObject = base.mapper.Map<Database>(model);
            var result = new ApiResult<string>();
            if (saveObject.Id == 0)
            {
                saveObject.ChangeTime = DateTime.Now;
                saveObject.IsDeleted = false;
                databaseDb.Insert(saveObject);
                result.IsSuccess = true;
                result.Data = Pubconst.MESSAGEADDSUCCESS;
            }
            else
            {
                saveObject.ChangeTime = DateTime.Now;
                saveObject.IsDeleted = false;
                databaseDb.Update(saveObject);
                result.IsSuccess = true;
                result.Data = Pubconst.MESSAGEADDSUCCESS;
            }
            return result;
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("deletedatabase")]
        public ActionResult<ApiResult<bool>> DeleteDatabase([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IndexViewModel>>(model);
                var exp = Expressionable.Create<Database>();
                foreach (var item in list)
                {
                    exp.Or(it => it.Id == item.Id);
                }
                databaseDb.Update(it => new Database() { IsDeleted = true }, exp.ToExpression());
            }
            result.IsSuccess = true;
            return result;
        }
    }
}
