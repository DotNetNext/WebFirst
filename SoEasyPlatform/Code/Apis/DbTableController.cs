using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
namespace SoEasyPlatform.Code.Apis
{
    public class DbTableController : BaseController
    {
        public DbTableController(IMapper mapper) : base(mapper)
        {
        }

        [HttpPost]
        [ExceptionFilter]
        [Route("GetTableList")]
        public ActionResult<ApiResult<List<DbTableInfo>>> GetTableList()
        {
            ApiResult<List<DbTableInfo>> result = new ApiResult<List<DbTableInfo>>();
            result.Data=Db.DbMaintenance.GetTableInfoList();
            result.IsSuccess = true;
            return result;
        }
    }
}
