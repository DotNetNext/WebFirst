using System;
using System.Collections.Generic;
using System.IO;
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

            InitMenu(db);

            InitConnection(db);

            InitTemplate(db);

            InitFileInfo(db);

            InitCodeTable(db);

            InitProject(db);

 
        }

 
        private static void InitProject(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<Project>();
        }

        private static void InitCodeTable(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<CodeTable>();
            db.CodeFirst.InitTables<CodeType>();
            db.CodeFirst.InitTables<CodeColumns>();
            if (!db.Queryable<CodeType>().Any())
            {
                var list = new List<CodeType>
                {
                    new CodeType{  Name="int",
                                  CSharepType=CSharpDataType.@int.ToString(),
                                  DbType=new  DbTypeInfo[]{
                                                            new DbTypeInfo() { Name="int" },
                                                            new DbTypeInfo() { Name="int4" },
                                                            new DbTypeInfo() { Name="number", Length=9, DecimalDigits=0 }
                                    }
                    },
                    new CodeType{
                                  Name="string10",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="varchar",Length=10} 
                                }
                    },
                    new CodeType{
                                  Name="string36",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="varchar",Length=36} 
                                }
                    },
                    new CodeType{
                                  Name="string100",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="varchar",Length=100} 
                                }
                    },
                    new CodeType{
                                  Name="string200",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="varchar",Length=100} 
                                }
                    },
                    new CodeType{
                                  Name="string500",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="varchar",Length=500} 
                                }
                    },
                    new CodeType{
                                  Name="string2000",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="varchar",Length=2000} 
                                }
                    },
                    new CodeType{
                                  Name="nString10",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="nvarchar",Length=10},
                                                           new DbTypeInfo(){  Name="varchar",Length=10}
                                }
                    },
                    new CodeType{
                                  Name="nString36",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="nvarchar",Length=36},
                                                           new DbTypeInfo(){  Name="varchar",Length=36}
                                }
                    },
                    new CodeType{
                                  Name="nString100",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="nvarchar",Length=100},
                                                           new DbTypeInfo(){  Name="varchar",Length=100}
                                }
                    },
                    new CodeType{
                                  Name="nString200",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="nvarchar",Length=200},
                                                           new DbTypeInfo(){  Name="varchar",Length=200}
                                }
                    },
                    new CodeType{
                                  Name="nString500",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="nvarchar",Length=500},
                                                           new DbTypeInfo(){  Name="varchar",Length=500}
                                }
                    },
                    new CodeType{
                                  Name="nString2000",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="nvarchar",Length=2000},
                                                           new DbTypeInfo(){  Name="varchar",Length=2000}
                                }
                    },
                    new CodeType{
                                  Name="maxString",
                                  CSharepType=CSharpDataType.@string.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="text"},
                                                           new DbTypeInfo(){  Name="clob"}
                                }
                    },
                    new CodeType{
                                  Name="bool",
                                  CSharepType=CSharpDataType.@bool.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="bit"},
                                                           new DbTypeInfo(){  Name="number", Length=1},
                                                           new DbTypeInfo(){  Name="boolean" }
                                }
                    },
                    new CodeType{
                                  Name="DateTime",
                                  CSharepType=CSharpDataType.DateTime.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="datetime"},
                                                           new DbTypeInfo(){  Name="date"}
                                                          
                                }
                    },
                    new CodeType{
                                  Name="decimal_18_4",
                                  CSharepType=CSharpDataType.@decimal.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="decimal",Length=18, DecimalDigits=4},
                                                           new DbTypeInfo(){  Name="number",Length=18, DecimalDigits=4}
                                }
                    },
                    new CodeType{
                                  Name="decimal_18_2",
                                  CSharepType=CSharpDataType.@decimal.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="decimal",Length=18, DecimalDigits=2},
                                                           new DbTypeInfo(){  Name="number",Length=18, DecimalDigits=2}
                                }
                    },
                    new CodeType{
                                  Name="guid",
                                  CSharepType=CSharpDataType.Guid.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="uniqueidentifier"},
                                                           new DbTypeInfo(){  Name="guid"},
                                                           new DbTypeInfo(){  Name="char",Length=36}
                                }
                    },
                    new CodeType{
                                  Name="byte",
                                  CSharepType=CSharpDataType.Guid.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="tinyint"},
                                                           new DbTypeInfo(){  Name="varbit"},
                                                           new DbTypeInfo(){  Name="number",Length=3}
                                }
                    },
                    new CodeType{
                                  Name="short",
                                  CSharepType=CSharpDataType.@short.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="short"},
                                                           new DbTypeInfo(){  Name="int2"},
                                                           new DbTypeInfo(){  Name="int16"},
                                                           new DbTypeInfo(){  Name="smallint"},
                                                           new DbTypeInfo(){  Name="number",Length=5}

                                }
                    },
                    new CodeType{
                                  Name="long",
                                  CSharepType=CSharpDataType.@long.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="long"},
                                                           new DbTypeInfo(){  Name="int8"},
                                                           new DbTypeInfo(){  Name="int64"},
                                                           new DbTypeInfo(){  Name="bigint"},
                                                           new DbTypeInfo(){  Name="number",Length=19}

                                }
                    },
                    new CodeType{
                                  Name="byteArray",
                                  CSharepType=CSharpDataType.byteArray.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="clob"},
                                                           new DbTypeInfo(){  Name="bit"},
                                                           new DbTypeInfo(){  Name="longblob"},
                                                           new DbTypeInfo(){  Name="binary"}

                                }
                    }

                };
                foreach (var item in list)
                {
                    item.Sort = 1000;
                }
                db.Insertable(list).ExecuteCommand();
            }
        }

        private static void InitConnection(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<Database>();
        }

        private static void InitFileInfo(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<FileInfo>();
            if (db.Queryable<FileInfo>().Count() == 0)
            {
                AddFile1(db);
                AddFile2(db);
                AddFile3(db);
                AddFile4(db);
            }
        }

        private static void AddFile1(SqlSugarClient db)
        {
            var temp = @"wwwroot\template\Lib1.txt";
            var temp2 = @"wwwroot\template\Lib1_1.txt";
            var directory = Directory.GetCurrentDirectory();
            var d1 = new FileInfo()
            {
                ChangeTime = DateTime.Now,
                Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)),
                Json = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)),
                Name = ".net 标准类库",
                Id = 1,
                IsInit = true,
                IsDeleted = false,
                Suffix = "csproj"

            };
            db.Insertable(d1).ExecuteCommand();
        }
        private static void AddFile2(SqlSugarClient db)
        {
            var temp = @"wwwroot\template\Lib2.txt";
            var temp2 = @"wwwroot\template\Lib1_1.txt";
            var directory = Directory.GetCurrentDirectory();
            var d1 = new FileInfo()
            {
                ChangeTime = DateTime.Now,
                Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)),
                Json = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)),
                Name = ".net core 5.0 类库",
                IsInit = true,
                Id = 1,
                IsDeleted = false,
                Suffix = "csproj"

            };
            db.Insertable(d1).ExecuteCommand();
        }
        private static void AddFile3(SqlSugarClient db)
        {
            var temp = @"wwwroot\template\Lib2.txt";
            var temp2 = @"wwwroot\template\Lib1_1.txt";
            var directory = Directory.GetCurrentDirectory();
            var d1 = new FileInfo()
            {
                ChangeTime = DateTime.Now,
                Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)).Replace("net5.0", "netcoreapp3.1"),
                Json = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)),
                Name = ".net core 3.1 类库",
                IsInit = true,
                Id = 1,
                IsDeleted = false,
                Suffix = "csproj"

            };
            db.Insertable(d1).ExecuteCommand();
        }
        private static void AddFile4(SqlSugarClient db)
        {
            var temp = @"wwwroot\template\DbContext.txt";
            var temp2 = @"wwwroot\template\DbContext_1.txt";
            var directory = Directory.GetCurrentDirectory();
            var d1 = new FileInfo()
            {
                ChangeTime = DateTime.Now,
                Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)).Replace("net5.0", "netcoreapp3.1"),
                Json = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)),
                Name = "DbContext.cs",
                IsInit = true,
                Id = 1,
                IsDeleted = false,
                Suffix = "cs"

            };
            db.Insertable(d1).ExecuteCommand();
        }

        private static void InitTemplate(SqlSugarClient db)
        {
            //db.DbMaintenance.DropTable("Template");
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
                var temp = @"wwwroot\template\Entity01.txt";
                var directory = Directory.GetCurrentDirectory();
                db.Insertable(new Template()
                {
                    ChangeTime = DateTime.Now,
                    Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)),
                    TemplateTypeName = "实体",
                    Sort = 0,
                    TemplateTypeId = 1,
                    Title = "SqlSugar默认实体模版"

                }).ExecuteCommand();


                var temp2 = @"wwwroot\template\biz.txt";
                db.Insertable(new Template()
                {
                    ChangeTime = DateTime.Now,
                    Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)),
                    TemplateTypeName = "业务",
                    Sort = 0,
                    TemplateTypeId = 2,
                    Title = "默认业务模版"

                }).ExecuteCommand();
            }
        }

        private static void InitMenu(SqlSugarClient db)
        {
            if (db.DbMaintenance.IsAnyTable("Menu"))
            {
                db.DbMaintenance.DropTable("Menu");
            }
            db.CodeFirst.InitTables<Menu>();
            db.Insertable(new List<Menu>()
                {
                    new Menu()
                     {
                      MenuName="代码管理",
                      Icon="fa fa-edit",
                        Child=new List<Menu>()
                        {
                             new Menu{ MenuName="配置数据库" , Url="/Database"},
                             new Menu{ MenuName="CodeFirst" , Url="/CodeFirst"},
                             new Menu{ MenuName="DbFirst" , Url="/DbFirst"},
                             new Menu{ MenuName="配置业务",Url="/BIZ" },
                             new Menu{ MenuName="配置客户端" ,Url="/Web"},
                             new Menu{ MenuName="方案管理" , Url="/Project"} ,
                             new Menu{ MenuName="模版管理" , Url="/Template"} ,
                            new Menu{ MenuName="文件管理" , Url="/FileInfo"} ,
                             new Menu{ MenuName="解决方案 ×" , Url="/Solution"},
                             new Menu{ MenuName="数据类型" , Url="/CodeType"},
                        }
                     }
                    ,
                     new Menu()
                     {
                        MenuName="数据库管理",
                        Icon="fa fa-bar-chart-o",
                        Child=new List<Menu>()
                        {
                             new Menu{ MenuName="待定1" },
                             new Menu{ MenuName="待定2" },

                        }
                     }
                         ,
                     new Menu()
                     {
                        MenuName="全部教程",
                        Icon="fa fa-bar-chart-o",
                        Child=new List<Menu>()
                        {
                             new Menu{ MenuName="WebFirst",Url="https://www.donet5.com/Doc/11?src=webfirst" },
                             new Menu{ MenuName="SqlSugar ORM" ,Url="https://www.donet5.com/Home/Doc?src=webfirst" },

                        }
                     }

                })
                .AddSubList(it => it.Child.First().ParentId).ExecuteCommand();
        }
    }
}
