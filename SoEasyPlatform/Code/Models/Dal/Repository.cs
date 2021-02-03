using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class Repository<T> : SimpleClient<T> where T : class, new()
    {
        public Repository(ISqlSugarClient context = null) : base(context)//注意这里要有默认值等于null
        {
            if (context == null)
            {
                base.Context = GetInstance();
            }
        }

        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db;
            var callValue=CallContext.GetData("db");
            if (callValue == null)
            {
                db = new SqlSugarClient(new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.Sqlite,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = "DataSource=" + AppContext.BaseDirectory + @"\database\sqlite.db"
                });
                db.QueryFilter.Add(new TableFilterItem<CodeTable>(it => it.IsDeleted==false));
                db.QueryFilter.Add(new TableFilterItem<Nuget>(it => it.IsDeleted == false));
                CallContext.SetData("db", db);
            }
            else 
            {
                db = callValue as SqlSugarClient;
            }
            return db;
        }
        public static SqlSugarClient GetInstance(DbType type,string connection)
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                DbType = type,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                ConnectionString = connection,
                ConfigureExternalServices=new ConfigureExternalServices() { 
                 RazorService=new RazorService()
                }
            });
        }
    }
}
