using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SoEasyPlatform 
{
    public class TemplateHelper
    {
        public const string EntityKey = "SoEasyPlatform.Entity";
        public static string  GetTemplateValue<T>(string key,string template,T model) 
        {
            var result = Engine.Razor.RunCompile(key, template, model.GetType(), model);
            return result;
        }
    }
}
