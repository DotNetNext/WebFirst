using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace SoEasyPlatform 
{
    public class TemplateHelper
    {
        public const string EntityKey = "SoEasyPlatform.Entity";
        public static string  GetTemplateValue<T>(string key,string template,T model) 
        {
            try
            {
                var result = Engine.Razor.RunCompile(template, key, model.GetType(), model);
                return result;
            }
            catch (Exception ex)
            {
                var message = ex.Message.Substring(801,700);
                throw new Exception("模版解析出错,"+message);
            }
        }
    }
}
