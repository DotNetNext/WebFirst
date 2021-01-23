using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace SoEasyPlatform
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public BaseController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        protected IMapper mapper;
        protected Repository<Menu> MenuDb => new Repository<Menu>();
        protected Repository<Database> databaseDb => new Repository<Database>();
        protected Repository<NetVersion> NetVersionDb => new Repository<NetVersion>();
        protected Repository<Nuget> NugetDb => new Repository<Nuget>();
        protected Repository<Template> TemplateDb => new Repository<Template>();
        protected Repository<TemplateType> TemplateTypeDb => new Repository<TemplateType>();
        protected Repository<CodeTable> CodeTableDb => new Repository<CodeTable>();
        protected Repository<CodeColumns> CodeColumnsDb => new Repository<CodeColumns>();
        protected Repository<CodeType> CodeTypeDb => new Repository<CodeType>();
        protected SqlSugarClient Db => Repository<object>.GetInstance();
        /// <summary>
        /// 验证数据库逻辑是否符合要求
        /// </summary>
        /// <param name="primaryValue"></param>
        /// <returns></returns>
        protected JsonResult ValidateModel(object primaryValue = null)
        {
            JsonResult errorResult = null;
            var validateItem = this.HttpContext.Items?.FirstOrDefault(it => it.Key.ToString() == Pubconst.ITEMKEY);
            if (validateItem != null && validateItem.Value.Value is ValidateUnique)
            {
                var unItem = (validateItem.Value.Value as ValidateUnique);
                var queryable = MenuDb.AsQueryable().AS(unItem.TableName).Where(new List<IConditionalModel>() {
                    new ConditionalModel(){ FieldName=unItem.DbColumnName,FieldValue=unItem.Value +""}
                });
                if (unItem.PrimaryKey != null && primaryValue != null && primaryValue.ToString() != "0")
                {
                    queryable.Where(new List<IConditionalModel>() {
                    new ConditionalModel(){ FieldName=unItem.PrimaryKey , ConditionalType=ConditionalType.NoEqual,FieldValue=primaryValue +""}
                });
                }
                if (queryable.Any())
                {
                    errorResult = new JsonResult(new ApiResult<List<KeyValuePair<string, string>>>()
                    {
                        Data = new List<KeyValuePair<string, string>>() {
                            new KeyValuePair<string, string>(unItem.FieldName,unItem.Message)
                        },
                        IsSuccess = false,
                        IsKeyValuePair = true
                    });
                }
            }

            return errorResult;
        }


        protected SqlSugarClient GetTryDb(Database db)
        {
            try
            {
                using (var Db = Repository<object>.GetInstance(db.DbType, db.Connection))
                {
                    Db.Open();
                    return Db;
                }
            }
            catch  
            {

                throw new Exception(db.Connection+" "+db.DbType+"无法连接到数据库，请认真检查DbType和连接字符串");
            }
        }

        protected SqlSugarClient GetTryDb(int dbId)
        {
           var database= databaseDb.GetById(dbId);
            return GetTryDb(database);
        }

        protected bool IsConnectionDb(Database db)
        {
            try
            {
                using (var Db = Repository<object>.GetInstance(db.DbType, db.Connection))
                {
                    Db.Ado.CommandTimeOut = 1;
                    Db.Open();
                    Db.DbMaintenance.CreateDatabase();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        protected void Check(bool isOk, string message)
        {
            if (isOk)
                throw new Exception(message);
        }
    }
}
