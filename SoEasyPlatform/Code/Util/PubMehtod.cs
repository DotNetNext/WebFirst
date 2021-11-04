using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform
{
    public class PubMehtod
    {
        public static string GetCsharpName(string dbColumnName)
        {
            if (dbColumnName.Contains("_"))
            {
                dbColumnName = dbColumnName.TrimEnd('_');
                dbColumnName = dbColumnName.TrimStart('_');
                var array = dbColumnName.Split('_').Select(it=>GetFirstUpper(it,true)).ToArray();
                return string.Join("", array);
            }
            else
            {
                return GetFirstUpper(dbColumnName);
            }
        }

        private static string GetFirstUpper(string dbColumnName,bool islower=false)
        {
            if (dbColumnName == null)
                return null;
            if (islower)
            {
                return dbColumnName.Substring(0, 1).ToUpper() + dbColumnName.Substring(1).ToLower();
            }
            else 
            {
                return dbColumnName.Substring(0, 1).ToUpper() + dbColumnName.Substring(1);
            }
        }
    }
}
