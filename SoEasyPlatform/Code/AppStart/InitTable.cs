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
                             new Menu{ MenuName="数据库连接" , Url="/"},
                             new Menu{ MenuName="实体" , Url="/Entity"},
                             new Menu{ MenuName="业务",Url="/BLL" },
                             new Menu{ MenuName="WEB框架" ,Url="/Web"},
                             new Menu{ MenuName="模版管理" , Url="/Template"} ,
                             new Menu{ MenuName="项目管理" , Url="/Project"} ,
                             new Menu{ MenuName="解决方案" , Url="/Solution"} ,
                        }
                     }
                    ,
                     new Menu()
                     {
                        MenuName="数据库维护",
                        Icon="fa fa-bar-chart-o",
                        Child=new List<Menu>()
                        {
                             new Menu{ MenuName="数据迁移" },
                             new Menu{ MenuName="数据表管理" }
                        }
                     } 
                   

                })
                .AddSubList(it=>it.Child.First().ParentId).ExecuteReturnPrimaryKey();
 
            db.CodeFirst.InitTables<DBConnection>();
            db.DbMaintenance.DropTable("Template");
            db.CodeFirst.InitTables<Template, TemplateType>();
            if (db.Queryable<Template>().Count() == 0)
            {
                db.Insertable(new List<TemplateType>()
                {
                     new  TemplateType(){   Name="实体" },
                     new  TemplateType(){   Name="业务" },
                     new  TemplateType(){   Name="Web" }

                }).ExecuteCommand();
            }

            if (db.Queryable<Template>().Count() == 0) 
            {
                db.Insertable(new Template()
                {
                     ChangeTime=DateTime.Now,
                     Content =  RazorFirst.DefaultRazorClassTemplate ,
                     IsSystemData=true,
                     TemplateTypeName="实体",
                     Sort=0,
                     TemplateTypeId=1,
                     Title="SqlSugar默认实体模版"

            }).ExecuteCommand();
            }
        }
    }
}
