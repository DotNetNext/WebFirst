using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
namespace SoEasyPlatform
{
    public class InitTable
    {
        public static void Start()
        {
            var db = Repository<Menu>.GetInstance();
            db.DbMaintenance.CreateDatabase();
            if (db.DbMaintenance.IsAnyTable("Menu"))
            {
                db.DbMaintenance.DropTable("Menu");
            }
            db.CodeFirst.InitTables<Menu>();
            db.Insertable(new List<Menu>()
                {
                    new Menu()
                     {
                      MenuName="代码生成",
                      Icon="fa fa-edit",
                        Child=new List<Menu>()
                        {
                             new Menu{ MenuName="实体" },
                             new Menu{ MenuName="业务" },
                             new Menu{ MenuName="WEB框架" }
                        }
                     }
                    ,
                     new Menu()
                     {
                        MenuName="数据库管理",
                        Icon="fa fa-bar-chart-o",
                        Child=new List<Menu>()
                        {
                             new Menu{ MenuName="数据迁移" },
                             new Menu{ MenuName="数据连接管理" },
                             new Menu{ MenuName="数据表管理" }
                        }
                     } 
                   

                })
                .AddSubList(it=>it.Child.First().ParentId).ExecuteReturnPrimaryKey();

            db.CodeFirst.InitTables<DBConnection>();
        }
    }
}
