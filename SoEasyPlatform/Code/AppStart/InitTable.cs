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
            InitMenu(db);

            InitConnection(db);

            InitTemplate(db);

            InitNetVersion(db);

            InitNuget(db);
        }

        private static void InitConnection(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<DBConnection>();
        }

        private static void InitNuget(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<Nuget>();
            if (db.Queryable<Nuget>().Count() == 0)
            {
                db.Insertable(new List<Nuget>() {
                     new Nuget(){
                        NetVersion=2,
                        Version="4.9.9.11",
                        Name="sqlSugar"
                     } }).ExecuteCommand();
                db.Insertable(new List<Nuget>() {
                     new Nuget(){
                        NetVersion=3,
                        Version="5.0.1.5",
                        Name="sqlSugar"
                     } }).ExecuteCommand();
                db.Insertable(new List<Nuget>() {
                     new Nuget(){
                        NetVersion=21,
                        Version="5.0.1.5",
                        Name="sqlSugarCore"
                     } }).ExecuteCommand();
                db.Insertable(new List<Nuget>() {
                     new Nuget(){
                        NetVersion=31,
                        Version="5.0.1.5",
                        Name="sqlSugarCore"
                     } }).ExecuteCommand();
            }
        }

        private static void InitNetVersion(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<NetVersion>();
            if (db.Queryable<NetVersion>().Count() == 0)
            {
                db.Insertable(new List<NetVersion>() {
                     new NetVersion(){
                        Id=1,
                        Name="不生成类库，不更新类库"
                     },
                     new NetVersion(){
                        Id=2,
                        Name=".NET Freamework 4.0 生成类库 ，更新类库"
                     },
                     new NetVersion(){
                        Id=3,
                        Name=".NET Freamework 4.5 生成类库 ，更新类库"
                     },

                    new NetVersion(){
                        Id=21,
                        Name=".NET Core 3.1 生成类库 ，更新类库"
                     },

                    new NetVersion(){
                        Id=31,
                        Name=".NET 5 生成类库 ，更新类库"
                     }

                }).ExecuteCommand();

            }
        }

        private static void InitTemplate(SqlSugarClient db)
        {
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
                    ChangeTime = DateTime.Now,
                    Content = RazorFirst.DefaultRazorClassTemplate,
                    IsSystemData = true,
                    TemplateTypeName = "实体",
                    Sort = 0,
                    TemplateTypeId = 1,
                    Title = "SqlSugar默认实体模版"

                }).ExecuteCommand();
            }
        }

        private static void InitMenu(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<Menu>();
            db.Insertable(new List<Menu>()
                {
                    new Menu()
                     {
                      MenuName="WebFirst",
                      Icon="fa fa-edit",
                        Child=new List<Menu>()
                        {
                             new Menu{ MenuName="数据库管理" , Url="/"},
                             new Menu{ MenuName="配置虚拟类" , Url="/CodeTable"},
                             new Menu{ MenuName="生成实体" , Url="/CodeEntity"},
                             new Menu{ MenuName="生成业务",Url="/BIZ" },
                             new Menu{ MenuName="生成Web" ,Url="/Web"},
                             new Menu{ MenuName="模版管理" , Url="/Template"} ,
                             new Menu{ MenuName="方案管理" , Url="/Project"} ,
                             new Menu{ MenuName="Nuget管理" , Url="/Nuget"} ,
                             new Menu{ MenuName="解决方案" , Url="/Solution"},
                             new Menu{ MenuName="数据类型" , Url="/CodeType"},
                        }
                     }
                    ,
                     new Menu()
                     {
                        MenuName="敬请期待",
                        Icon="fa fa-bar-chart-o",
                        Child=new List<Menu>()
                        {
                             new Menu{ MenuName="待定1" },
                             new Menu{ MenuName="待定2" },
                       
                        }
                     }


                })
                .AddSubList(it => it.Child.First().ParentId).ExecuteReturnPrimaryKey();
        }
    }
}
