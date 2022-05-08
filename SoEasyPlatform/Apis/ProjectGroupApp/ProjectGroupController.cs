using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SoEasyPlatform.Models.ViewModel;
using SqlSugar;

namespace SoEasyPlatform.Apis
{
    /// <summary>
    /// 虚拟类配置
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public partial class ProjectGroupController : BaseController
    {
        IMapper _mapper;
        public ProjectGroupController(IMapper mapper) : base(mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// 获取解决方案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetProjectGroupList")]
        public ActionResult<ApiResult<TableModel<ProjectGroupGridViewModel>>> GetProjectGroupList([FromForm] ProjectGroupViewModel model)
        {
            model.PageSize = 20;
            var result = new ApiResult<TableModel<ProjectGroupGridViewModel>>();
            result.Data = new TableModel<ProjectGroupGridViewModel>();
            int count = 0;
            var list = this.Db.Queryable<ProjectGroup>()
                .WhereIF(!string.IsNullOrEmpty(model.Name), it => it.Name.Contains(model.Name))
                .OrderBy(it => it.Sort)
                .OrderBy(it => it.Id)
                .ToPageList(model.PageIndex, model.PageSize, ref count);
            var codeGridList = mapper.Map<List<ProjectGroupGridViewModel>>(list);
            result.Data.Rows = codeGridList;
            result.Data.Total = count;
            result.Data.PageSize = model.PageSize;
            result.Data.PageNumber = model.PageIndex;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 添加解决方案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [Route("SaveProjectGroup")]
        public ActionResult<ApiResult<string>> SaveProjectGroup([FromForm] ProjectGroupViewModel model)
        {
            var result = new ApiResult<string>();
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var isNoUpdatProjectIds = model.ProjectNames == "noupdate";
            if (isNoUpdatProjectIds)
            {
                model.ProjectNames = "0";
            }
            ProjectGroup ProjectGroup = new ProjectGroup()
            {
                Id = Convert.ToInt32(model.Id),
                Name = model.Name,
                Sort = Convert.ToInt32(model.Sort),
                SolutionPath=model.SolutionPath,
                ProjectIds=model.ProjectNames.Split(',').Select(it=>Convert.ToInt32(it)).ToArray(),
                ProjectNames = String.Join(",", Db.Queryable<Project>().In(model.ProjectNames.Split(',').Select(it => Convert.ToInt32(it)).ToArray()).Select(it => it.ProjentName).ToArray())
            };
            var x = Db.Storageable(ProjectGroup).ToStorage();
            x.AsUpdateable.IgnoreColumnsIF(isNoUpdatProjectIds, it=>new { it.ProjectIds,it.ProjectNames}).ExecuteCommand();
            x.AsInsertable.ExecuteCommand();
            result.IsSuccess = true;
            result.Data = x.InsertList.Any() ? Pubconst.MESSAGEADDSUCCESS : Pubconst.MESSAGESAVESUCCESS;
            return result;
        }


        /// <summary>
        /// 删除解决方案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteProjectGroup")]
        public ActionResult<ApiResult<bool>> deleteProjectGroup([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProjectGroupViewModel>>(model);
                Db.Deleteable<ProjectGroup>().In(list.Select(it => it.Id).ToList()).ExecuteCommand();
            }
            result.IsSuccess = true;
            return result;
        }
    }
}
