using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : BaseController
    {
        public ProjectController(IMapper mapper) : base(mapper)
        {

        }
        /// <summary>
        /// 获取系统列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getProjectlist")]
        public ActionResult<ApiResult<TableModel<ProjectGridViewModel>>> GetProjectList([FromForm] ProjectViewModel model)
        {
            var result = new ApiResult<TableModel<ProjectGridViewModel>>();
            result.Data = new TableModel<ProjectGridViewModel>();
            int count = 0;
            var list = ProjectDb.AsSugarClient().Queryable<Project>()
                .WhereIF(!string.IsNullOrEmpty(model.ProjentName), it => it.ProjentName.Contains(model.ProjentName))
                .WhereIF(model.ModelId >0, it => it.ModelId == model.ModelId)
                .OrderBy(it => new { it.ProjentName })
                .Select(it=> new ProjectGridViewModel
                {
                  FileSuffix=it.FileSuffix,
                  Id=it.Id,
                  FileInfo=it.FileInfo,
                  ModelId=it.ModelId.GetConfigValue<TemplateType>(),
                  Path=it.Path,
                  ProjentName=it.ProjentName,
                  TemplateId1=it.TemplateId1.GetConfigValue<Template>(),
                  FileModel=it.FileModel
                })
                .Mapper(it=> {
                    if(!string.IsNullOrEmpty(it.FileInfo))
                    {
                        var ids = it.FileInfo.Split(',');
                        var name = Db.Queryable<FileInfo>().In(ids).Select(it => it.Name).ToList();
                        it.FileInfo = string.Join(",", name);
                    }
                })
                .ToPageList(model.PageIndex, model.PageSize, ref count);
            result.Data.Rows = list;
            result.Data.Total = count;
            result.Data.PageSize = model.PageSize;
            result.Data.PageNumber = model.PageIndex;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [FormValidateFilter]
        [Route("saveProject")]
        public ActionResult<ApiResult<string>> SaveProject([FromForm] ProjectViewModel model)
        {
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var saveObject = base.mapper.Map<Project>(model);
            var result = new ApiResult<string>();
            saveObject.IsDeleted = false;
            var x = Db.Storageable(saveObject).ToStorage();
            x.AsUpdateable.ExecuteCommand();
            x.AsInsertable.ExecuteCommand();
            result.Data = x.InsertList.Any() ? Pubconst.MESSAGEADDSUCCESS : Pubconst.MESSAGESAVESUCCESS;
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteProject")]
        public ActionResult<ApiResult<bool>> DeleteProject([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DatabaseViewModel>>(model);
                var isInit = Db.Queryable<Project>().In(list.Select(it=>it.Id).ToList()).Any(it => it.IsInit == true);
                base.Check(isInit, "无法删除初始化数据");
                var exp = Expressionable.Create<Project>();
                foreach (var item in list)
                {
                    exp.Or(it => it.Id == item.Id);
                }
                ProjectDb.Update(it => new Project() { IsDeleted = true }, exp.ToExpression());
            }
            result.IsSuccess = true;
            return result;
        }
    }
}
