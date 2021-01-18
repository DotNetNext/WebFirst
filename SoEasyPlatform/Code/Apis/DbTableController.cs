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
        public ActionResult<ApiResult<TableModel<DbTableViewModel>>> GetTableList(int? dbId)
        {
           var result =new  ApiResult<TableModel<DbTableViewModel>>();
            result.Data = new TableModel<DbTableViewModel>();
            var db = GetTryDb(databaseDb.GetById(dbId.Value));
            result.Data.Rows=mapper.Map<List<DbTableViewModel>> (db.DbMaintenance.GetTableInfoList());
            var codetable = CodeTableDb.GetList(it => it.DbId == dbId.Value);
            foreach (var item in result.Data.Rows)
            {
                item.IsImport = codetable.Any(it => it.TableName.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase)) ? "已导入" : "未导入";
            }
            result.IsSuccess = true;
            result.Data.PageNumber = 1;
            result.Data.PageSize = result.Data.Rows.Count;
            return result;
        }
    }
}
