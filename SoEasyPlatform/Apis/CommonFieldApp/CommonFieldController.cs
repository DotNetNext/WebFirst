using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace SoEasyPlatform.Apis
{
    [Route("api/[controller]")]
    [ExceptionFilter]
    [ApiController]
    public class CommonFiledController : BaseController
    {

        public CommonFiledController(IMapper mapper) : base(mapper)
        {
        }
        /// <summary>
        /// 获取所有公共字段
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCommonFieldList")]
        public ActionResult<ApiResult<TableModel<CommonFieldGridViewModel>>> GetCommonFieldList([FromForm] CommonFieldViewModel model)
        {
            var result = new ApiResult<TableModel<CommonFieldGridViewModel>>();
            result.Data = new TableModel<CommonFieldGridViewModel>();
            int count = 0;
            var list = CommonFieldDb.AsQueryable()
                .ToPageList(model.PageIndex, model.PageSize, ref count);
            result.Data.Rows = mapper.Map<List<CommonFieldGridViewModel>>(list);
            result.Data.Total = count;
            result.Data.PageSize = model.PageSize;
            result.Data.PageNumber = model.PageIndex;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [Route("SaveCommonField")]
        public ActionResult<ApiResult<string>> SaveCommonField([FromForm] CommonFieldViewModel model)
        {
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var saveObject = base.mapper.Map<CommonField>(model);
            var result = new ApiResult<string>();
            var s = Db.Storageable(saveObject).ToStorage();
            s.AsUpdateable.ExecuteCommand();
            s.AsInsertable.ExecuteCommand();
            result.IsSuccess = s.ErrorList.Count == 0;
            result.Data = result.IsSuccess ? "保存成功" : s.ErrorList.First().StorageMessage;
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteCommonField")]
        public ActionResult<ApiResult<bool>> DeleteCommonField([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CommonFieldViewModel>>(model);
                var isInit = Db.Queryable<CommonField>().In(list.Select(it=>it.Id).ToList()).Any(it => it.IsInit == true);
                base.Check(isInit, "无法删除初始化数据");
                var exp = Expressionable.Create<CommonField>();
                foreach (var item in list)
                {
                    exp.Or(it => it.Id == item.Id);
                }
                CommonFieldDb.Update(it => new CommonField() { IsDeleted = true }, exp.ToExpression());
            }
            result.IsSuccess = true;
            return result;
        }
    }
}

