using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace SoEasyPlatform
{
    /// <summary>
    /// 用来初始化整个项目主入口
    /// </summary>
    public partial class InitTable
    {
        SqlSugarClient db;
        private void InitProjects(SqlSugarClient db)
        {
            this.db = db;
            var directory = FileSugar.MergeUrl(Directory.GetCurrentDirectory(), "wwwroot", "template", "Projects");
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            var slnListPathList = Directory.GetDirectories(directory);
            if (slnListPathList == null) return;
            try
            {
                db.BeginTran();
                db.Updateable<ProjectGroup>().SetColumns(it => it.Description == "")
.Where(it => true)
.ExecuteCommand();
                foreach (var sln in slnListPathList)
                {
                    AddSln(sln);
                }
                db.CommitTran();
            }
            catch (System.Exception ex)
            {
                db.RollbackTran();
            }
        }
    }
}
