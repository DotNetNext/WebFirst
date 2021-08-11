using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
namespace SoEasyPlatform.Apis
{
    /// <summary>
    /// 数据库表操作
    /// </summary>
    public class DbTableController : BaseController
    {
        public DbTableController(IMapper mapper) : base(mapper)
        {
        }

        [HttpPost]
        [ExceptionFilter]
        [Route("GetTableList")]
        public ActionResult<ApiResult<TableModel<DbTableGridViewModel>>> GetTableList(int? dbId, [FromForm] DbTableViewModel model)
        {
           var result =new  ApiResult<TableModel<DbTableGridViewModel>>();
            result.Data = new TableModel<DbTableGridViewModel>();
            var db = GetTryDb(dbId.Value);
            result.Data.Rows=mapper.Map<List<DbTableGridViewModel>> (db.DbMaintenance.GetTableInfoList(false));
            var codetable = CodeTableDb.AsQueryable().Where(it => it.DbId == dbId.Value).OrderBy(it=>it.TableName).ToList();
            foreach (var item in result.Data.Rows)
            {
                item.IsImport = codetable.Any(it => it.TableName.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase)) ? "导入" : "";
            }
            if (!string.IsNullOrEmpty(model.name))
            {
                result.Data.Rows = result.Data.Rows.Where(it => it.Name.ToLower().Contains(model.name?.ToLower())).ToList();
            }
            if (model.typeId=="1")
            {
                result.Data.Rows = result.Data.Rows.Where(it => !string.IsNullOrEmpty(it.IsImport)).ToList();
            }
            if (model.typeId == "2")
            {
                result.Data.Rows = result.Data.Rows.Where(it => string.IsNullOrEmpty(it.IsImport)).ToList();
            }
            result.IsSuccess = true;
            result.Data.PageNumber = 1;
            result.Data.PageSize = result.Data.Rows.Count;
            return result;
        }
    }
}
