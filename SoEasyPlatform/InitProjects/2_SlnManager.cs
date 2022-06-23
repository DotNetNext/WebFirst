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
            projectNames = "";
            var slnName = System.IO.Path.GetFileName(sln);
            var groupdata = db.Queryable<ProjectGroup>().First(it => it.Name == slnName);
            ClearGroup(groupdata);
            ProjectGroup projectGroup = CreateEmptyProject();
            groupId = db.Insertable(projectGroup).ExecuteReturnIdentity();
            var ids = AddProjects(sln, slnName);
            projectGroup = UpdateProject(slnName, ids);
        }

        private ProjectGroup UpdateProject(string slnName, List<int> ids)
        {
            ProjectGroup projectGroup = new ProjectGroup()
            {
                ProjectIds = ids.ToArray(),
                Name = slnName,
                ProjectNames = projectNames.TrimEnd(','),
                SolutionPath =FileSugar.MergeUrl("c:\\Projects\\" + slnName),
                Sort = 100,
                Id = groupId
            };
            db.Updateable(projectGroup).ExecuteCommand();
            return projectGroup;
        }

        private static ProjectGroup CreateEmptyProject()
        {
            return new ProjectGroup()
            {
                ProjectIds = new int[] { },
                Name = "",
                ProjectNames = "",
                SolutionPath = "",
                Sort = 100
            };
        }

        private void ClearGroup(ProjectGroup groupdata)
        {
            if (groupdata != null)
            {

                db.Deleteable<ProjectGroup>(groupdata).ExecuteCommand();
                db.Deleteable<Project>().Where(it => it.SolutionId.Equals(groupdata.Id)).ExecuteCommand();
                db.Deleteable<FileInfo>().Where(it => it.SolutionId.Equals(groupdata.Id)).ExecuteCommand();
                db.Deleteable<Template>().Where(it => it.SolutionId.Equals(groupdata.Id)).ExecuteCommand();
            }
        }
    }
}
