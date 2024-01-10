using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar.IOC;
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
            var db =DbScoped.Sugar;
            if (db.QueryFilter.GeFilterList?.Any() == false)
            {
                db.QueryFilter.Add(new TableFilterItem<CodeTable>(it => it.IsDeleted == false));
                db.QueryFilter.Add(new TableFilterItem<FileInfo>(it => it.IsDeleted == false));
                db.QueryFilter.Add(new TableFilterItem<Template>(it => it.IsDeleted == false));
                db.QueryFilter.Add(new TableFilterItem<Project>(it => it.IsDeleted == false));
                db.QueryFilter.Add(new TableFilterItem<CommonField>(it => it.IsDeleted == false));
                db.QueryFilter.Add(new TableFilterItem<TagProperty>(it => it.IsDeleted == false));
            }
            db.Aop.OnError = exp =>
            {
                var logPath = FileSugar.MergeUrl(AppContext.BaseDirectory, "log","log" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                FileSugar.AppendText(DateTime.Now+"", exp.Sql);
                FileSugar.AppendText(logPath, exp.Sql);
                FileSugar.AppendText("", exp.Sql);
            };
            db.Aop.OnLogExecuting = (sql,p) =>
            {

            };
            return db;
        }
        public static SqlSugarClient GetInstance(DbType type,string connection)
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                DbType = type,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                ConnectionString = connection
            }, db => {
                db.Aop.OnLogExecuting = (s, p) =>
                {
                    Console.WriteLine(s);
                };
            });
        }
    }
}
