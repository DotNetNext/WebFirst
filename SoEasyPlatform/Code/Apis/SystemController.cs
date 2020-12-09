using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace SoEasyPlatform.Code.Apis
{
    /// <summary>
    /// 系统常用接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : BaseController
    {

        public SystemController(IMapper mapper)
        {
            base.mapper = mapper;
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
                    Id = ((int)type).ToString(),
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
        /// 获取.net版本
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        [Route("getnetversion")]
        public ActionResult<ApiResult<List<TreeModel>>> GetNetVersion()
        {
            List<TreeModel> trees = new List<TreeModel>();
            var databses = NetVersionDb.GetList();
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
        /// 获取nuget
        /// </summary>
        /// <param name="netVersion"></param>
        /// <returns></returns>

        [HttpPost]
        [AuthorizeFilter]
        [Route("getnuget")]
        public ActionResult<ApiResult<List<TreeModel>>> GetNuget(int? netVersion)
        {
            List<TreeModel> trees = new List<TreeModel>();
            var databases = NugetDb.GetList(it=>it.IsDeleted == false);
            if (netVersion > 0) 
            {
                databases = databases.Where(it => it.NetVersion == netVersion.Value).ToList();
            }
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
        /// 获取.NET版本，不包含1
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        [Route("getgoodnetversion")]
        public ActionResult<ApiResult<List<TreeModel>>> GetGoodNetVersion()
        {
            List<TreeModel> trees = new List<TreeModel>();
            var databses = NetVersionDb.GetList(it=>it.Id!=1);
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
            var databses = TemplateDb.GetList(it => it.IsDeleted == false&&it.TemplateTypeId==type);
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
    }
}
