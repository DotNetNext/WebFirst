using RazorEngine;
using RazorEngine.Templating;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class RazorService:IRazorService  
    {
        public List<KeyValuePair<string, string>> GetClassStringList(string razorTemplate, List<RazorTableInfo> model)
        {
            if (model != null && model.Any())
            {
                var result = new List<KeyValuePair<string, string>>();
                var old = razorTemplate;
                foreach (var item in model)
                {
                    try
                    {
                        item.ClassName = item.DbTableName;//这边可以格式化类名
                        if (old.Contains("$格式化类名$")) 
                        {
                            var array = old.Split("$格式化类名$");
                            razorTemplate = array[0];
                            item.ClassName = GetClassName(item.ClassName, array[1]);
                        }
                        string key = "RazorService.GetClassStringList" + razorTemplate.Length;
                        var classString = Engine.Razor.RunCompile(razorTemplate, key, item.GetType(), item);
                        result.Add(new KeyValuePair<string, string>(item.ClassName, classString));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(item.DbTableName + " error ." + ex.Message);
                    }
                }
                return result;
            }
            else
            {
                return new List<KeyValuePair<string, string>>();
            }
        }

        private string GetClassName(string className, string format)
        {
            var old = format;
            try
            {
 
                format = "@(" + format.Replace("{表名}", "(@Model)") + ")";
                var key = "formatclassnamekey"+format;
                var classString = Engine.Razor.RunCompile(format, key, typeof(string), className);
                return classString;
            }
            catch(Exception ex)  
            {
               throw  new Exception(old+"格式错误");
            }
        }
    }
}
