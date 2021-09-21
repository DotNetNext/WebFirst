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
        /// <summary>
        /// 默认实体ID
        /// </summary>
        private int _entitytempId=0;
        /// <summary>
        /// 默认业务模版
        /// </summary>
        private int _biztempId = 0;
        /// <summary>
        /// 默认Web模版
        /// </summary>
        private int _webtempId = 0;
        /// <summary>
        /// 默认文件dbcontext
        /// </summary>
        private int _dbcontext = 0;
        /// <summary>
        /// 默认文件 net5 lib
        /// </summary>
        private int _net5lib = 0;
        /// <summary>
        /// Web模版需要的文件
        /// </summary>
        private List<int> _WebFiles = new List<int>();
        /// <summary>
        /// 默认命名空间
        /// </summary>
        private string _defaultNamespace = "WebFirst";
        public  void Start()
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

 
        private  void InitProject(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<Project>();
            if (db.Queryable<Project>().Count() == 0)
            {
                var pid = db.Insertable(new Project()
                {
                    FileSuffix = ".cs",
                    TemplateId1 = _entitytempId + "",
                    FileModel = "[{ \"name\":\"" + _defaultNamespace + ".Entities\",\"nuget\":[{ \"name\":\"SqlSugarCore\",\"version\":\"5.0.4\" }]}]",
                    FileInfo = _net5lib + "",
                    NameFormat = "Common\\{0}",
                    ProjentName = "【简单三层】_实体_Sugar",
                    Path = @"c:\" + _defaultNamespace + @"\Entites",
                    IsDeleted = false,
                    IsInit = true,
                    ModelId = 1
                }).ExecuteReturnIdentity();
                var pid2= db.Insertable(new Project()
                {
                    FileSuffix = ".cs",
                    NameFormat = "Common\\{0}",
                    TemplateId1 = _biztempId + "",
                    FileModel = "[{ \"name\":\""+_defaultNamespace+ ".Services\",\"nuget\":[{ \"name\":\"SqlSugarCore\",\"version\":\"5.0.4\" }]},{\"name\":\"DbContext\", \"name_space\":\"" + _defaultNamespace+ ".Services\" }]",
                    FileInfo = _net5lib + ","+_dbcontext,
                    ProjentName = "【简单三层】_方案_业务_Sugar",
                    Path = @"c:\" + _defaultNamespace + @"\Services",
                    IsDeleted = false,
                    IsInit = true,
                    ModelId = 2,
                    Reference=pid+""
                }).ExecuteReturnIdentity();
                db.Insertable(new Project()
                {
                    FileSuffix = ".cs",
                    TemplateId1 = _webtempId + "",
                    FileModel = "[{ \"name\":\"命名空间\",\"nuget\":[{ \"name\":\"SqlSugarCore\",\"version\":\"5.0.4\" },{ \"name\":\"Microsoft.AspNetCore.Mvc.NewtonsoftJson\",\"version\":\"5.0.10\" },{ \"name\":\"Swashbuckle.AspNetCore\",\"version\":\"5.6.3\" }]},{\"name\":\"Startup\", \"name_space\":\"命名空间\" },{\"name\":\"Program\", \"name_space\":\"命名空间\" },{\"name\":\"appsettings\", \"name_space\":\"命名空间\"},{\"name\":\"launchSettings\", \"name_space\":\"命名空间\"}]".Replace("命名空间",$"{_defaultNamespace}.Api"),
                    FileInfo =string.Join(",",_WebFiles),
                    ProjentName = "【简单三层】_方案_前端_Sugar",
                    NameFormat = "Common\\{0}",
                    Path = @"c:\" + _defaultNamespace + @"\Api",
                    IsDeleted = false,
                    IsInit = true,
                    ModelId = 3,
                    Reference = pid + ","+pid2
                }).ExecuteReturnIdentity();
            }
        }

        private  void InitCodeTable(SqlSugarClient db)
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
                                                            new DbTypeInfo() { Name="number", Length=9, DecimalDigits=0 },
                                                            new DbTypeInfo(){ Name="integer" }
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
                                  Name="ignore",
                                  CSharepType="建表忽略该类型字段，生成实体中@Model.IsIgnore 值为 true ",
                                  DbType=new DbTypeInfo[]{
                                                  
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
                                                           new DbTypeInfo(){  Name="varchar",Length=200} 
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
                                 Name="timestamp",
                                 CSharepType="byte[]",
                                 DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="timestamp"} 

                                }
                    },
                    new CodeType{
                                  Name="decimal_18_8",
                                  CSharepType=CSharpDataType.@decimal.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="decimal",Length=18, DecimalDigits=8},
                                                           new DbTypeInfo(){  Name="number",Length=18, DecimalDigits=8},
                                                           new DbTypeInfo(){  Name="numeric",Length=18, DecimalDigits=8}
                                }
                    },
                    new CodeType{
                                  Name="decimal_18_4",
                                  CSharepType=CSharpDataType.@decimal.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="decimal",Length=18, DecimalDigits=4},
                                                           new DbTypeInfo(){  Name="number",Length=18, DecimalDigits=4},
                                                           new DbTypeInfo(){  Name="money",Length=0, DecimalDigits=0}
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
                                  CSharepType=CSharpDataType.@byte.ToString(),
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
                                  CSharepType="byte[]",
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="clob"},
                                                           new DbTypeInfo(){  Name="bit"},
                                                           new DbTypeInfo(){  Name="longblob"},
                                                           new DbTypeInfo(){  Name="binary"}

                                }
                    },
                    new CodeType{
                                  Name="datetimeoffset",
                                  CSharepType=CSharpDataType.DateTimeOffset.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="datetimeoffset"}

                                }
                    },
                    new CodeType{
                                  Name="json_default",
                                  CSharepType=CSharpDataType.DateTimeOffset.ToString(),
                                  DbType=new DbTypeInfo[]{
                                                           new DbTypeInfo(){  Name="json"},
                                                           new DbTypeInfo(){  Name="varchar", Length=3999}
                                                           

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

        private  void InitConnection(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<Database>();
        }

        private  void InitFileInfo(SqlSugarClient db)
        {
            db.CodeFirst.InitTables<FileInfo>();
            if (db.Queryable<FileInfo>().Count() == 0)
            {
                AddFile1(db);
                AddFile2(db);
                AddFile2_1(db);
                AddFile2_1_1(db);
                AddFile3(db);
                AddFile4(db);
                AddFile5(db);
                AddFile6(db);
                AddFile7(db);
                AddFile8(db);
            }
        }

        private void AddFile8(SqlSugarClient db)
        {
            var temp = @"wwwroot\template\Api_LanJson.txt";
            var temp2 = @"wwwroot\template\Default_1.txt";
            var directory = Directory.GetCurrentDirectory();
            var d1 = new FileInfo()
            {
                ChangeTime = DateTime.Now,
                Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)),
                Json = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)).Replace("文件名", "Properties\\launchSettings"),
                Name = "API文件[launchSettings.json]",
                Id = 1,
                IsInit = true,
                IsDeleted = false,
                Suffix = "json"

            };
            var id = db.Insertable(d1).ExecuteReturnIdentity();
            _WebFiles.Add(id);
        }
        private void AddFile7(SqlSugarClient db)
        {
            var temp = @"wwwroot\template\Api_SettJson.txt";
            var temp2 = @"wwwroot\template\Default_1.txt";
            var directory = Directory.GetCurrentDirectory();
            var d1 = new FileInfo()
            {
                ChangeTime = DateTime.Now,
                Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)),
                Json = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)).Replace("文件名", "appsettings"),
                Name = "API文件[appsettings.json]",
                Id = 1,
                IsInit = true,
                IsDeleted = false,
                Suffix = "json"

            };
            var id = db.Insertable(d1).ExecuteReturnIdentity();
            _WebFiles.Add(id);
        }

        private void AddFile6(SqlSugarClient db)
        {
            var temp = @"wwwroot\template\Api_Program.txt";
            var temp2 = @"wwwroot\template\DbContext_1.txt";
            var directory = Directory.GetCurrentDirectory();
            var d1 = new FileInfo()
            {
                ChangeTime = DateTime.Now,
                Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)),
                Json = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)).Replace("DbContext", "Program"),
                Name = "API文件[Program]",
                Id = 1,
                IsInit = true,
                IsDeleted = false,
                Suffix = "cs"

            };
            var id = db.Insertable(d1).ExecuteReturnIdentity();
            _WebFiles.Add(id);
        }

        private void AddFile5(SqlSugarClient db)
        {
            var temp = @"wwwroot\template\Api_Startup.txt";
            var temp2 = @"wwwroot\template\DbContext_1.txt";
            var directory = Directory.GetCurrentDirectory();
            var d1 = new FileInfo()
            {
                ChangeTime = DateTime.Now,
                Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)),
                Json = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)).Replace("DbContext", "Startup"),
                Name = "API文件[Startup]",
                Id = 1,
                IsInit = true,
                IsDeleted = false,
                Suffix = "cs"

            };
            var id= db.Insertable(d1).ExecuteReturnIdentity();
            _WebFiles.Add(id);
        }

        private  void AddFile1(SqlSugarClient db)
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
        private  void AddFile2(SqlSugarClient db)
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
            _net5lib= db.Insertable(d1).ExecuteReturnIdentity();
        }
        private void AddFile2_1(SqlSugarClient db)
        {
            var temp = @"wwwroot\template\WebLib.txt";
            var temp2 = @"wwwroot\template\Lib1_1.txt";
            var directory = Directory.GetCurrentDirectory();
            var d1 = new FileInfo()
            {
                ChangeTime = DateTime.Now,
                Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)),
                Json = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)),
                Name = ".net core 5.0 web 项目",
                IsInit = true,
                Id = 1,
                IsDeleted = false,
                Suffix = "csproj"

            };
            var id = db.Insertable(d1).ExecuteReturnIdentity();
            _WebFiles.Add(id);
        }
        private void AddFile2_1_1(SqlSugarClient db)
        {
            var temp = @"wwwroot\template\WebLib2.txt";
            var temp2 = @"wwwroot\template\Lib1_1.txt";
            var directory = Directory.GetCurrentDirectory();
            var d1 = new FileInfo()
            {
                ChangeTime = DateTime.Now,
                Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)),
                Json = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)).Replace("net5.0", "netcoreapp3.1"),
                Name = ".net core 3.1  web 项目",
                IsInit = true,
                Id = 1,
                IsDeleted = false,
                Suffix = "csproj"

            };
            var id = db.Insertable(d1).ExecuteReturnIdentity();
        }
        private  void AddFile3(SqlSugarClient db)
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
        private  void AddFile4(SqlSugarClient db)
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
            _dbcontext=db.Insertable(d1).ExecuteReturnIdentity();
        }

        private  void InitTemplate(SqlSugarClient db)
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
                _entitytempId= db.Insertable(new Template()
                {
                    ChangeTime = DateTime.Now,
                    Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp)),
                    TemplateTypeName = "实体",
                    Sort = 0,
                    TemplateTypeId = 1,
                    Title = "【简单三层】_模版_实体_Sugar",
                    IsInit=true

                }).ExecuteReturnIdentity();


                var temp2 = @"wwwroot\template\biz.txt";
               _biztempId= db.Insertable(new Template()
                {
                    ChangeTime = DateTime.Now,
                    Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp2)),
                    TemplateTypeName = "业务",
                    Sort = 0,
                    TemplateTypeId = 2,
                    Title = "【简单三层】_模版_业务_Sugar",
                    IsInit=true

                }).ExecuteReturnIdentity();


                var temp3 = @"wwwroot\template\web.txt";
                _webtempId = db.Insertable(new Template()
                {
                    ChangeTime = DateTime.Now,
                    Content = FileSugar.FileToString(FileSugar.MergeUrl(directory, temp3)),
                    TemplateTypeName = "业务",
                    Sort = 0,
                    TemplateTypeId = 2,
                    Title = "【简单三层】_模版_前端_Sugar",
                    IsInit = true

                }).ExecuteReturnIdentity();
            }
        }

        private  void InitMenu(SqlSugarClient db)
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
                             new Menu{ MenuName="配置实体（类建表模式）" , Url="/CodeFirst"},
                             new Menu{ MenuName="配置实体（表建类模式）" , Url="/DbFirst"},
                             new Menu{ MenuName="配置业务",Url="/BIZ" },
                             new Menu{ MenuName="配置前端" ,Url="/Web"},
                             new Menu{ MenuName="方案管理" , Url="/Project"} ,
                             new Menu{ MenuName="模版管理" , Url="/Template"} ,
                            new Menu{ MenuName="文件管理" , Url="/FileInfo"} ,
                             new Menu{ MenuName="云方案 ×" , Url="/Solution"},
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
                             new Menu{ MenuName="Sugar.IOC" ,Url="https://www.donet5.com/Doc/10/2250?src=webfirst" },
                             new Menu{ MenuName="打赏作者" ,Url="https://www.donet5.com/Doc/28/2357?src=webfirst" },
                             new Menu{ MenuName="商务合作" ,Url="https://www.donet5.com/Doc/28/2358?src=webfirst" },
                        }
                     }

                })
                .AddSubList(it => it.Child.First().ParentId).ExecuteCommand();
        }
    }
}
