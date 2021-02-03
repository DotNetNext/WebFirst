using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace SoEasyPlatform.Code.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class NugetController : BaseController
    {
        public NugetController(IMapper mapper) : base(mapper)
        {

        }
        /// <summary>
        /// 获取系统列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getnugetlist")]
        public ActionResult<ApiResult<TableModel<NugetGridViewModel>>> GetNugetList([FromForm] NugetViewModel model)
        {
            var result = new ApiResult<TableModel<NugetGridViewModel>>();
            result.Data = new TableModel<NugetGridViewModel>();
            int count = 0;
            var list = NugetDb.AsSugarClient().Queryable<Nuget, NetVersion>(
                 (it, nvs) => new  JoinQueryInfos(
                       JoinType.Left,it.NetVersion==nvs.Id
                     )
                )
                .WhereIF(!string.IsNullOrEmpty(model.Name), it => it.Name.Contains(model.Name))
                .WhereIF(model.NetVersion>0, it => it.NetVersion==model.NetVersion.Value)
                .OrderBy(it=>new { it.Name,it.Version})
                .Select((it,nvs)=>new NugetGridViewModel() { 
                     Id=SqlFunc.GetSelfAndAutoFill(it.Id),
                     NetVersionName=nvs.Name
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
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [Route("savenuget")]
        public ActionResult<ApiResult<string>> SaveNuget([FromForm] NugetViewModel model)
        {
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var saveObject = base.mapper.Map<Nuget>(model);
            var result = new ApiResult<string>();
            if (saveObject.Id == 0)
            {
                saveObject.IsDeleted = false;
                NugetDb.Insert(saveObject);
                result.IsSuccess = true;
                result.Data = Pubconst.MESSAGEADDSUCCESS;
            }
            else
            {
                saveObject.IsDeleted = false;
                NugetDb.Update(saveObject);
                result.IsSuccess = true;
                result.Data = Pubconst.MESSAGEADDSUCCESS;
            }
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("deletenuget")]
        public ActionResult<ApiResult<bool>> DeleteNuget([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DatabaseViewModel>>(model);
                var exp = Expressionable.Create<Nuget>();
                foreach (var item in list)
                {
                    exp.Or(it => it.Id == item.Id);
                }
                NugetDb.Update(it => new Nuget() { IsDeleted = true }, exp.ToExpression());
            }
            result.IsSuccess = true;
            return result;
        }
    }
}
