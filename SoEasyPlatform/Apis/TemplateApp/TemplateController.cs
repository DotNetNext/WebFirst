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
    [ApiController]
    public class TemplateController : BaseController
    {

        public TemplateController(IMapper mapper) : base(mapper)
        {
        }
        /// <summary>
        /// 获取所有模版
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTemplateList")]
        public ActionResult<ApiResult<TableModel<TemplateGridViewModel>>> GetTemplateList([FromForm] TemplateViewModel model)
        {
            var result = new ApiResult<TableModel<TemplateGridViewModel>>();
            result.Data = new TableModel<TemplateGridViewModel>();
            int count = 0;
            var list = TemplateDb.AsQueryable()
                .ToPageList(model.PageIndex, model.PageSize, ref count);
            result.Data.Rows = mapper.Map<List<TemplateGridViewModel>>(list);
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
        [Route("SaveTemplate")]
        public ActionResult<ApiResult<string>> SaveTemplate([FromForm] TemplateViewModel model)
        {
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var saveObject = base.mapper.Map<Template>(model);
            saveObject.ChangeTime = DateTime.Now;
            var result = new ApiResult<string>();
            var s = Db.Storageable(saveObject)
                    .SplitError(it => it.Any(y => y.Id == it.Item.Id && y.TemplateTypeId != it.Item.TemplateTypeId), "类型不能修改")
                    .SplitInsert(it => it.Item.Id == 0)
                    .SplitUpdate(it => it.Item.Id > 0).ToStorage();
            s.AsUpdateable.UpdateColumns(it => new { it.ChangeTime, it.Title, it.Content }).ExecuteCommand();
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
        [Route("deletetemplate")]
        public ActionResult<ApiResult<bool>> DeleteTemplate([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TemplateViewModel>>(model);
                var isInit = Db.Queryable<Template>().In(list.Select(it=>it.Id).ToList()).Any(it => it.IsInit == true);
                base.Check(isInit, "无法删除初始化数据");
                var exp = Expressionable.Create<Template>();
                foreach (var item in list)
                {
                    exp.Or(it => it.Id == item.Id);
                }
                TemplateDb.Update(it => new Template() { IsDeleted = true }, exp.ToExpression());
            }
            result.IsSuccess = true;
            return result;
        }
    }
}

