using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SoEasyPlatform
{
    /// <summary>
    /// 表单验证
    /// </summary>
    public class FormValidateFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 控制器中加了该属性的方法中代码执行之前该方法。
        /// 所以可以用做权限校验。
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            List<KeyValuePair<string, string>> errorParamters = new List<KeyValuePair<string, string>>();
            foreach (var item in context.ActionArguments)
            {
                if (item.Value == null)
                {
                    errorParamters.Add(new KeyValuePair<string, string>("", "OnActionExecuting.Parametres error"));
                    break;
                }
                if (item.Value is  IView)
                {
                    var properties = item.Value.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        var cusAttr = property.GetCustomAttributes(true);
                        var lenAttr = cusAttr.FirstOrDefault(it => it is ValidateLength) as ValidateLength;
                        var reqAttr = cusAttr.FirstOrDefault(it => it is ValidateReduired) as ValidateReduired;
                        var uniqueAttr = cusAttr.FirstOrDefault(it => it is ValidateUnique) as ValidateUnique;
                        var equalAttr = cusAttr.FirstOrDefault(it => it is ValidateEqual) as ValidateEqual;

                        var md5Convert = cusAttr.FirstOrDefault(it => it is ConvertMd5) as ConvertMd5;
                        var properyName = cusAttr.FirstOrDefault(it => it is PropertyName) as PropertyName;


                        var name = properyName == null ? property.Name : properyName.Name;
                        var propertyValue = property.GetValue(item.Value);
                        if (reqAttr != null)
                        {
                            if (propertyValue == null)
                            {
                                errorParamters.Add(new KeyValuePair<string, string>(property.Name, $"{name}不能为空"));
                            }
                        }
                        if (lenAttr != null && item.Value != null)
                        {
                            var value = propertyValue + "";
                            if (value.Length < lenAttr.Min || value.Length > lenAttr.Max)
                            {
                                if (!errorParamters.Any(it => it.Key == property.Name))
                                    errorParamters.Add(new KeyValuePair<string, string>(property.Name, $"{name}的长度应该在{lenAttr.Min}-{lenAttr.Max}之间"));
                            }
                        }

                        if (equalAttr != null)
                        {
                            object equalValue = properties.First(it => it.Name == equalAttr.EqualPropertyName).GetValue(item.Value);
                            if (propertyValue?.ToString() != equalValue?.ToString())
                            {
                                errorParamters.Add(new KeyValuePair<string, string>(property.Name, $"{name}错误"));
                            }
                        }

                        if (uniqueAttr != null)
                        {
                            uniqueAttr.Value = propertyValue;
                            uniqueAttr.Message = name + "已经存在";
                            uniqueAttr.FieldName = property.Name;
                            context.HttpContext.Items.Add(Pubconst.ITEMKEY, uniqueAttr);
                        }

                        if (md5Convert != null)
                        {
                            md5Convert.FieldName = property.Name;
                            context.HttpContext.Items.Add(Pubconst.ITEMCONVERT, md5Convert);
                        }
                    }
                }
            }
            if (errorParamters.Any())
            {
                context.Result = new JsonResult(new ApiResult<List<KeyValuePair<string, string>>>()
                {
                    Data = errorParamters,
                    IsSuccess = false,
                    IsKeyValuePair = true
                });
            }
            base.OnActionExecuting(context);
        }
        /// <summary>
        /// 控制器中加了该属性的方法执行完成后才会来执行该方法。
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

        }
        /// <summary>
        /// 控制器中加了该属性的方法执行完成后才会来执行该方法。比OnActionExecuted()方法还晚执行。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {

            return base.OnResultExecutionAsync(context, next);
        }
    }

    /// <summary>
    /// 用户权限
    /// </summary>
    public class AuthorizeFilter: ActionFilterAttribute
    {
        /// <summary>
        /// 控制器中加了该属性的方法中代码执行之前该方法。
        /// 所以可以用做权限校验。
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
        /// <summary>
        /// 控制器中加了该属性的方法执行完成后才会来执行该方法。
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

        }
        /// <summary>
        /// 控制器中加了该属性的方法执行完成后才会来执行该方法。比OnActionExecuted()方法还晚执行。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {

            return base.OnResultExecutionAsync(context, next);
        }
    }
}
