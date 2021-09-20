using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace SoEasyPlatform.Apis
{
    /// <summary>
    /// 系统常用接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : BaseController
    {

        public SystemController(IMapper mapper) : base(mapper)
        {

        }

        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getmenu")]
        public ActionResult<ApiResult<List<Menu>>> GetMenu()
        {
            var list = MenuDb.AsQueryable().ToTree(it => it.Child, it => it.ParentId, null);
            var result = new ApiResult<List<Menu>>();
            result.Data = list;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        [Route("getdbtype")]
        public ActionResult<ApiResult<List<TreeModel>>> GetDbType()
        {
            List<TreeModel> trees = new List<TreeModel>();
            foreach (DbType type in Enum.GetValues(typeof(DbType)))
            {
                trees.Add(new TreeModel()
                {
                    Id = type.ToString(),
                    Title = type.ToString(),
                    IsSelectable = true
                });
            }
            ApiResult<List<TreeModel>> result = new ApiResult<List<TreeModel>>();
            result.Data = trees;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [AuthorizeFilter]
        [Route("getdatabase")]
        public ActionResult<ApiResult<List<TreeModel>>> GetDatabase()
        {
            List<TreeModel> trees = new List<TreeModel>();
            var databses = databaseDb.GetList(it => it.IsDeleted == false);
            foreach (var db in databses)
            {
                trees.Add(new TreeModel()
                {
                    Id = db.Id.ToString(),
                    Title = db.Desc,
                    IsSelectable = true
                });
            }
            ApiResult<List<TreeModel>> result = new ApiResult<List<TreeModel>>();
            result.Data = trees;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="netVersion"></param>
        /// <returns></returns>

        [HttpPost]
        [AuthorizeFilter]
        [Route("GetFileInfo")]
        public ActionResult<ApiResult<List<TreeModel>>> GetFileInfo()
        {
            List<TreeModel> trees = new List<TreeModel>();
            var databases = FileInfoDb.GetList(it=>it.IsDeleted == false);
            foreach (var db in databases)
            {
                trees.Add(new TreeModel()
                {
                    Id = db.Id.ToString(),
                    Title = db.Name,
                    IsSelectable = true
                });
            }
            ApiResult<List<TreeModel>> result = new ApiResult<List<TreeModel>>();
            result.Data = trees;
            result.IsSuccess = true;
            return result;
        }
 


        /// <summary>
        /// 获取方案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        [Route("GetProject")]
        public ActionResult<ApiResult<List<TreeModel>>> GetProject(int typeId)
        {
            List<TreeModel> trees = new List<TreeModel>();
            var databses = ProjectDb.GetList(it => it.ModelId.Equals(typeId));
            foreach (var db in databses)
            {
                trees.Add(new TreeModel()
                {
                    Id = db.Id.ToString(),
                    Title = db.ProjentName,
                    IsSelectable = true
                });
            }
            ApiResult<List<TreeModel>> result = new ApiResult<List<TreeModel>>();
            result.Data = trees;
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        /// 获取方案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        [Route("GetProjectNoMyId")]
        public ActionResult<ApiResult<List<TreeModel>>> GetProjectNoMyId(int id)
        {
            List<TreeModel> trees = new List<TreeModel>();
            var databses = ProjectDb.GetList(it => it.Id!=id);
            foreach (var db in databses)
            {
                trees.Add(new TreeModel()
                {
                    Id = db.Id.ToString(),
                    Title = db.ProjentName,
                    IsSelectable = true
                });
            }
            ApiResult<List<TreeModel>> result = new ApiResult<List<TreeModel>>();
            result.Data = trees;
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        /// 根据类型获取模版
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        [Route("gettemplate")]
        public ActionResult<ApiResult<List<TreeModel>>> GetTemplate(int type)
        {
            List<TreeModel> trees = new List<TreeModel>();
            var databses = TemplateDb.GetList(it=>it.TemplateTypeId==type||type==0);
            foreach (var db in databses)
            {
                trees.Add(new TreeModel()
                {
                    Id = db.Id.ToString(),
                    Title = db.Title,
                    IsSelectable = true
                });
            }
            ApiResult<List<TreeModel>> result = new ApiResult<List<TreeModel>>();
            result.Data = trees;
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        /// 获取模版类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        [Route("gettemplatetype")]
        public ActionResult<ApiResult<List<TreeModel>>> GetTemplateType(int type)
        {
            List<TreeModel> trees = new List<TreeModel>();
            var databses = TemplateTypeDb.GetList();
            foreach (var db in databses)
            {
                trees.Add(new TreeModel()
                {
                    Id = db.Id.ToString(),
                    Title = db.Name,
                    IsSelectable = true
                });
            }
            ApiResult<List<TreeModel>> result = new ApiResult<List<TreeModel>>();
            result.Data = trees;
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        /// 获取数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        [Route("getdatatype")]
        public ActionResult<ApiResult<List<TreeModel>>> GetDataType(int type)
        {
            List<TreeModel> trees = new List<TreeModel>();
            var datas = CodeTypeDb.AsQueryable().OrderBy(it=>new { it.Sort,it.Id }).ToList();
            foreach (var data in datas)
            {
                trees.Add(new TreeModel()
                {
                    Id = data.Id.ToString(),
                    Title = data.Name,
                    IsSelectable = true
                });
            }
            ApiResult<List<TreeModel>> result = new ApiResult<List<TreeModel>>();
            result.Data = trees;
            result.IsSuccess = true;
            return result;
        }
    }
}
