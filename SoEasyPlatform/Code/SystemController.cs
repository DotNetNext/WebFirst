using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SoEasyPlatform.Code
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

    }
}
