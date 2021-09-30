using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform.Apis 
{
    /// <summary>
    /// 数据库导入虚拟类相关逻辑
    /// </summary>
    public partial class CodeTableController : BaseController
    {

        private void Export(string model, SqlSugarClient tableDb)
        {
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CodeTableViewModel>>(model);
            var oldList = CodeTableDb.AsQueryable().In(list.Select(it => it.Id).ToList()).ToList();
            List<EntitiesGen> genList = GetGenList(oldList, CodeTypeDb.GetList(), tableDb.CurrentConnectionConfig.DbType);
            List<DataTable> datatables = new List<DataTable>();
            foreach (var item in genList)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("列名");
                dt.Columns.Add("列描述");
                dt.Columns.Add("列类型");
                dt.Columns.Add("实体型");
                dt.Columns.Add("表名"); ;
                dt.Columns.Add("表描述");
                foreach (var it in item.PropertyGens)
                {
                    var dr = dt.NewRow();
                    dr["列名"] = it.DbColumnName;
                    dr["列描述"] = it.Description;
                    dr["列类型"] = it.DbType;
                    dr["实体型"] = it.Type;
                    dr["表名"] = item.TableName; ;
                    dr["表描述"] = item.Description;
                    dt.Rows.Add(dr);
                }
                datatables.Add(dt);
            }
        }
    }
}
