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
            groupdata = groupdata==null?CreateEmptyProject():groupdata;
            if (groupdata.Id == 0)
            {
                groupId = db.Insertable(groupdata).ExecuteReturnIdentity();
            }
            else 
            {
                groupId = groupdata.Id;
            }
            var ids = AddProjects(sln, slnName);
            UpdateProjectGroup(slnName, ids);
        }

        private ProjectGroup UpdateProjectGroup(string slnName, List<int> ids)
        {
            ProjectGroup projectGroup = new ProjectGroup()
            {
                ProjectIds = ids.ToArray(),
                Name = slnName,
                ProjectNames = projectNames.TrimEnd(','),
                SolutionPath =FileSugar.MergeUrl("c:\\Projects\\" + slnName),
                Sort = 100,
                Id = groupId,
                Description= $"<a style='display: block;width: 200px;' href='https://www.donet5.com/Doc/11/2433' target='_bank' >启用文件同步中,点击关闭</a>"
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

                //db.Deleteable<ProjectGroup>(groupdata).ExecuteCommand();
                db.Deleteable<Project>().Where(it => it.SolutionId.Equals(groupdata.Id)).ExecuteCommand();
                db.Deleteable<FileInfo>().Where(it => it.SolutionId.Equals(groupdata.Id)).ExecuteCommand();
                db.Deleteable<Template>().Where(it => it.SolutionId.Equals(groupdata.Id)).ExecuteCommand();
            }
        }
    }
}
