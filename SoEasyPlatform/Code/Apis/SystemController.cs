using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace SoEasyPlatform
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : BaseController
    {

        public SystemController(IMapper mapper)
        {
            base.mapper = mapper;
        }

        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getmenu")]
        public ActionResult<ApiResult<List<Menu>>> GetMenu()
        {
            var list = MenuDb.AsQueryable().ToTree(it => it.Child, it => it.ParentId, null);
            var result = new ApiResult<List<Menu>>();
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 获取系统列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getdbconnection")]
        public ActionResult<ApiResult<TableModel<DBConnectionGridViewModel>>> GetDbConnection([FromForm] DBConnectionViewModel model)
        {
            var result = new ApiResult<TableModel<DBConnectionGridViewModel>>();
            result.Data = new TableModel<DBConnectionGridViewModel>();
            int count = 0;
            var list = DBConnectionDb.AsQueryable().ToPageList(model.PageIndex, model.PageSize, ref count);
            result.Data.Rows = base.mapper.Map<List<DBConnectionGridViewModel>>(list);
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
        [Route("savedbconnection")]
        public ActionResult<ApiResult<string>> SaveDbConnection([FromForm] DBConnectionViewModel model)
        {
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var saveObject =base.mapper.Map<DBConnection>(model);
            var result = new ApiResult<string>();
            if (saveObject.Id == 0)
            {
                saveObject.ChangeTime = DateTime.Now;
                saveObject.IsDeleted = false;
                DBConnectionDb.Insert(saveObject);
                result.IsSuccess = true;
                result.Data = Pubconst.MESSAGEADDSUCCESS;
            }
            else
            {
                saveObject.ChangeTime = DateTime.Now;
                saveObject.IsDeleted = false;
                DBConnectionDb.Update(saveObject);
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
        [Route("deletedbconnection")]
        public ActionResult<ApiResult<bool>> DeleteDbconnection([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DBConnectionViewModel>>(model);
                var exp = Expressionable.Create<DBConnection>();
                foreach (var item in list)
                {
                    exp.Or(it => it.Id == item.Id);
                }
                DBConnectionDb.Update(it => new DBConnection() { IsDeleted = true }, exp.ToExpression());
            }
            return result;
        }

    }
}
