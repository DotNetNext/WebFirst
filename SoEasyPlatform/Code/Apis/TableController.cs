using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SoEasyPlatform.Code.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : BaseController
    {

        /// <summary>
        /// 获取系统列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ExceptionFilter]
        [Route("gettable")]
        public ActionResult<ApiResult<TableModel<TableGridViewModel>>> GetTable([FromForm] TableViewModel model)
        {
            var result = new ApiResult<TableModel<TableGridViewModel>>();
            result.Data = new TableModel<TableGridViewModel>();
            var listDb = DBConnectionDb.GetList();
            var db = listDb.FirstOrDefault(it => it.Id == model.Database);
            if (db == null)
                return result;
            int count = 0;
            var sqlsugarDb = base.GetTryDb(db);
            var tableList= sqlsugarDb.DbMaintenance.GetTableInfoList(false);
            foreach (var item in tableList)
            {
                TableGridViewModel tgv = new TableGridViewModel()
                {
                    Database = db.Desc,
                    TableDesc = item.Description,
                    TableName = item.Name
                };
                if (result.Data.Rows == null)
                    result.Data.Rows = new List<TableGridViewModel>();
                result.Data.Rows.Add(tgv);
            }
            result.Data.Total = count;
            result.Data.PageSize = int.MaxValue;
            result.Data.PageNumber = 1;
            result.IsSuccess = true;
            return result;
        }

    }
}
