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
    public class EntityController : BaseController
    {

        /// <summary>
        /// 获取系统列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ExceptionFilter]
        [Route("gettable")]
        public ActionResult<ApiResult<TableModel<EntityGridViewModel>>> GetTable([FromForm] EntityViewModel model)
        {
            var result = new ApiResult<TableModel<EntityGridViewModel>>();
            result.Data = new TableModel<EntityGridViewModel>();
            var listDb = databaseDb.GetList();
            var db = listDb.FirstOrDefault(it => it.Id == model.Database);
            if (db == null)
            {
                result.Data.Total = 0;
                result.Data.PageSize = int.MaxValue;
                result.Data.PageNumber = 1;
                result.IsSuccess = true;
                result.Data.Rows = new List<EntityGridViewModel>();
                return result;
            }
            int count = 0;
            var sqlsugarDb = base.GetTryDb(db);
            var tableList = sqlsugarDb.DbMaintenance.GetTableInfoList(false);
            if (model.TableName != null && tableList != null) 
            {
                tableList = tableList.Where(it => it.Name.ToLower().Contains(model.TableName.ToLower())).ToList();
            }
            foreach (var item in tableList)
            {
                EntityGridViewModel tgv = new EntityGridViewModel()
                {
                    Database = db.Desc,
                    TableDesc = item.Description,
                    TableName = item.Name,
                    Id = tableList.IndexOf(item) + 1
                };
                if (result.Data.Rows == null)
                    result.Data.Rows = new List<EntityGridViewModel>();
                result.Data.Rows.Add(tgv);
            }
            result.Data.Total = count;
            result.Data.PageSize = int.MaxValue;
            result.Data.PageNumber = 1;
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        /// 生成实体
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [ExceptionFilter]
        [Route("createfile")]
        public ActionResult<ApiResult<bool>> CreateFile([FromForm] TableToTemplateViewModel model, int databaseId)
        {
            var result = new ApiResult<bool>();
            var listDb = databaseDb.GetList();
            var db = listDb.FirstOrDefault(it => it.Id == databaseId);
            var sqlsugarDb = base.GetTryDb(db);
            var data = TemplateDb.GetById(model.TemplateId1);
            var tables = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EntityGridViewModel>>(model.Tables);
            var tableArray = tables.Select(it => it.TableName).ToArray();
            var content = data.Content;
            if (model.FileFormat != null) 
            {
               content=content+ "$格式化类名$" + model.FileFormat;
            }
            sqlsugarDb.DbFirst.Where(tableArray).UseRazorAnalysis(content).CreateClassFile(model.Path);
            result.IsSuccess = true;
            return result;
        }
    }
}
