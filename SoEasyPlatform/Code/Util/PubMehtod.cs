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
                var array = dbColumnName.Split('_').Select(it=>GetFirstUpper(it)).ToArray();
                return string.Join("", array);
            }
            else
            {
                return GetFirstUpper(dbColumnName);
            }
        }

        private static string GetFirstUpper(string dbColumnName)
        {
            if (dbColumnName == null)
                return null;
            return dbColumnName.Substring(0, 1).ToUpper() + dbColumnName.Substring(1);
        }
    }
}
