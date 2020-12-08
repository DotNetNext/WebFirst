using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace SoEasyPlatform
{
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


        [HttpPost]
        [AuthorizeFilter]
        [Route("getdatabase")]
        public ActionResult<ApiResult<List<TreeModel>>> GetDatabase()
        {
            List<TreeModel> trees = new List<TreeModel>();
            var databses = connectionDb.GetList(it => it.IsDeleted == false);
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


        /// <summary>
        /// 获取系统列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getdbconnection")]
        public ActionResult<ApiResult<TableModel<DBConnectionGridViewModel>>> GetDbConnection([FromForm] IndexViewModel model)
        {
            var result = new ApiResult<TableModel<DBConnectionGridViewModel>>();
            result.Data = new TableModel<DBConnectionGridViewModel>();
            int count = 0;
            var list =connectionDb.AsQueryable()
                .Where(it=>it.IsDeleted==false)
                .WhereIF(!string.IsNullOrEmpty(model.Desc),it=>it.Desc.Contains(model.Desc))
                .ToPageList(model.PageIndex, model.PageSize, ref count);
            result.Data.Rows = base.mapper.Map<List<DBConnectionGridViewModel>>(list);
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
        [Route("savedbconnection")]
        public ActionResult<ApiResult<string>> SaveDbConnection([FromForm] IndexViewModel model)
        {
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var saveObject = base.mapper.Map<Database>(model);
            var result = new ApiResult<string>();
            if (saveObject.Id == 0)
            {
                saveObject.ChangeTime = DateTime.Now;
                saveObject.IsDeleted = false;
                connectionDb.Insert(saveObject);
                result.IsSuccess = true;
                result.Data = Pubconst.MESSAGEADDSUCCESS;
            }
            else
            {
                saveObject.ChangeTime = DateTime.Now;
                saveObject.IsDeleted = false;
                connectionDb.Update(saveObject);
                result.IsSuccess = true;
                result.Data = Pubconst.MESSAGEADDSUCCESS;
            }
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("deletedbconnection")]
        public ActionResult<ApiResult<bool>> DeleteDbconnection([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IndexViewModel>>(model);
                var exp = Expressionable.Create<Database>();
                foreach (var item in list)
                {
                    exp.Or(it => it.Id == item.Id);
                }
                connectionDb.Update(it => new Database() { IsDeleted = true }, exp.ToExpression());
            }
            result.IsSuccess = true;
            return result;
        }

    }
}
