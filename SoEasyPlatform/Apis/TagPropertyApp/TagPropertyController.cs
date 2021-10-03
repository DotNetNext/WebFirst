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
    public class TagPropertyController : BaseController
    {

        public TagPropertyController(IMapper mapper) : base(mapper)
        {
        }
        /// <summary>
        /// 获取所有模版
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTagPropertyList")]
        public ActionResult<ApiResult<TableModel<TagPropertyGridViewModel>>> GetTagPropertyList([FromForm] TagPropertyViewModel model)
        {
            var result = new ApiResult<TableModel<TagPropertyGridViewModel>>();
            result.Data = new TableModel<TagPropertyGridViewModel>();
            int count = 0;
            var list = TagPropertyDb.AsQueryable()
                .ToPageList(model.PageIndex, model.PageSize, ref count);
            result.Data.Rows = mapper.Map<List<TagPropertyGridViewModel>>(list);
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
        [Route("SaveTagProperty")]
        public ActionResult<ApiResult<string>> SaveTagProperty([FromForm] TagPropertyViewModel model)
        {
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var saveObject = base.mapper.Map<TagProperty>(model);
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
        [Route("deleteTagProperty")]
        public ActionResult<ApiResult<bool>> DeleteTagProperty([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagPropertyViewModel>>(model);
                //var isInit = Db.Queryable<TagProperty>().In(list.Select(it=>it.Id).ToList()).Any(it => it.IsInit == true);
                //base.Check(isInit, "无法删除初始化数据");
                var exp = Expressionable.Create<TagProperty>();
                foreach (var item in list)
                {
                    exp.Or(it => it.Id == item.Id);
                }
                TagPropertyDb.Update(it => new TagProperty() { IsDeleted = true }, exp.ToExpression());
            }
            result.IsSuccess = true;
            return result;
        }
    }
}

