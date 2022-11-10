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
    [Route("api/[controller]")]
    [ExceptionFilter]
    [ApiController]
    public class FileInfoController : BaseController
    {
        public FileInfoController(IMapper mapper) : base(mapper)
        {

        }
        /// <summary>
        /// 获取系统列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getFileInfolist")]
        public ActionResult<ApiResult<TableModel<FileInfoGridViewModel>>> GetFileInfoList([FromForm] FileInfoViewModel model)
        {
            var result = new ApiResult<TableModel<FileInfoGridViewModel>>
            {
                Data = new TableModel<FileInfoGridViewModel>()
            };
            int count = 0;
            var list = Db.Queryable<FileInfo>()
                .WhereIF(!string.IsNullOrEmpty(model.Name), it => it.Name.Contains(model.Name))
                .OrderBy(it=>new { it.Name})
                .Select<FileInfoGridViewModel>()
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
        [Route("saveFileInfo")]
        public ActionResult<ApiResult<string>> SaveFileInfo([FromForm] FileInfoViewModel model)
        {
            CheckInitData(model.Id);
            JsonResult errorResult = base.ValidateModel(model.Id);
            if (errorResult != null) return errorResult;
            var saveObject = base.mapper.Map<FileInfo>(model);
            var result = new ApiResult<string>();
            model.Name = model.Name.Trim();
            ValidateFileInfo(saveObject);
            saveObject.IsDeleted = false;
            var x=Db.Storageable(saveObject).ToStorage();
            x.AsUpdateable.ExecuteCommand();
            x.AsInsertable.ExecuteCommand();
            result.Data =x.InsertList.Any()? Pubconst.MESSAGEADDSUCCESS:Pubconst.MESSAGESAVESUCCESS;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteFileInfo")]
        public ActionResult<ApiResult<bool>> DeleteFileInfo([FromForm] string model)
        {
            var result = new ApiResult<bool>();
            if (!string.IsNullOrEmpty(model))
            {
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DatabaseViewModel>>(model);
                var exp = Expressionable.Create<FileInfo>();
                foreach (var item in list)
                {
                    CheckInitData(item.Id);
                    exp.Or(it => it.Id == item.Id);
                }
                FileInfoDb.Update(it => new FileInfo() { IsDeleted = true }, exp.ToExpression());
            }
            result.IsSuccess = true;
            return result;
        }


        /// <summary>
        /// 获取填充模版
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRazorModel")]
        public ActionResult<ApiResult<string>> GetRazorModel([FromForm]string Id)
        {
            var result = new ApiResult<string>();
            result.Data += "[";
            if (!string.IsNullOrEmpty(Id))
            {
                var ids = Id.Split(',').Select(it=>it.Trim()).ToArray();
                var jsons= FileInfoDb.AsQueryable().Where(it => ids.Contains(it.Id.ToString())).Select(it=>it.Json).ToList();
                result.Data += string.Join(",", jsons);
            }
            result.IsSuccess = true;
            result.Data += "]";
            return result;
        }


        private void CheckInitData(int? id)
        {
            if (id == null)
                return;
            if (FileInfoDb.GetById(id.Value).IsInit)
            {
                throw new Exception("初始化数据无法修改");
            }
        }


        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="saveObject"></param>
        private void ValidateFileInfo(FileInfo saveObject)
        {
            try
            {
               var data= Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(saveObject.Json).name;
            }
            catch (Exception)
            {

                throw  new Exception("格式错误,在下面正确格式上添加节点： <br>  { name:\"文件名\" }");
            }
        }
    }
}
