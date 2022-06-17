using Microsoft.AspNetCore.Builder;
using SoEasyPlatform;
using SqlSugar.IOC;

var builder = WebApplication.CreateBuilder(args);
Services.AddServices(builder.Services);
builder.Services.AddSqlSugar(new SqlSugar.IOC.IocConfig()
{
    ConfigId = "master1",
    DbType = IocDbType.Sqlite,
    IsAutoCloseConnection = true,
    ConnectionString = "DataSource=" + Startup.CurrentDirectory + @"\database\sqlite.db"
});
builder.Services.ConfigurationSugar(db =>
{
    if (!db.ConfigQuery.Any())
    {
        db.ConfigQuery.SetTable<Template>(it => it.Id, it => it.Title);
        db.ConfigQuery.SetTable<TemplateType>(it => it.Id, it => it.Name);
    }
});

var app = builder.Build();
Configures.AddConfigure(app, app.Environment);

app.Run();