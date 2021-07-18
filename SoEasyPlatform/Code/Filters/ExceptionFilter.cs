using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform
{
    /// <summary>
    /// 异常过滤器，记录异常日志。
    /// </summary>
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        /// <summary>
        ///如果方法中处理了异常，则不会进入该方法。
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {

            context.ExceptionHandled = false;
            context.Result = new JsonResult(new ApiResult<string>()
            {
                Data = context.Exception.Message,
                Message=context.Exception.Message,
                IsSuccess = false 
            });
        }
    }
}
