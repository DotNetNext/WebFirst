using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SugarSite.Enties;

namespace SoEasyPlatform.Code
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
 
        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getmenu")]
        public ActionResult<ApiResult<List<Menu>>> GetMenu()
        {
            var dal=new Repository<Menu>();
            var list = dal.AsQueryable().ToTree(it=>it.Child,it=>it.ParentId,null);
            var result = new ApiResult<List<Menu>>();
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }
    }
}
