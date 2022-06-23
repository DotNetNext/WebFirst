using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SoEasyPlatform
{
    /// <summary>
    ///项目解决方案管理
    /// </summary>
    public partial class InitTable
    {
        int groupId = 0;
        private void AddSln(string sln)
        {
            var slnName = System.IO.Path.GetFileName(sln);
            string projectNames = "";
            var groupdata = db.Queryable<ProjectGroup>().First(it => it.Name == slnName);
            if (groupdata != null)
            {
                ClearGroup(groupdata);
            }
            ProjectGroup projectGroup = new ProjectGroup()
            {
                ProjectIds = new int[] { },
                Name = "",
                ProjectNames = "",
                SolutionPath = "",
                Sort = 100
            };
            groupId = db.Insertable(projectGroup).ExecuteReturnIdentity();
            var ids = AddProjects(sln, slnName);
            projectGroup = new ProjectGroup()
            {
                ProjectIds = ids.ToArray(),
                Name = slnName,
                ProjectNames = projectNames,
                SolutionPath = "c:\\Projects\\" + slnName,
                Sort = 100,
                Id = groupId
            };
            db.Updateable(projectGroup).ExecuteCommand();
        }

        private void ClearGroup(ProjectGroup groupdata)
        {
            db.Deleteable<ProjectGroup>(groupdata).ExecuteCommand();
            db.Deleteable<Project>().Where(it => it.SolutionId.Equals(groupdata.Id)).ExecuteCommand();
            db.Deleteable<FileInfo>().Where(it => it.SolutionId.Equals(groupdata.Id)).ExecuteCommand();
        }

    }
}
