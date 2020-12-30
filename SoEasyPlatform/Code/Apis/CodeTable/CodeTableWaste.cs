using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class CodeTableWaste
    {

        public static void AutoFillTable(CodeTable dbTable)
        {
            if (string.IsNullOrEmpty(dbTable.TableName))
            {
                dbTable.TableName = dbTable.ClassName;
            }
            if (string.IsNullOrEmpty(dbTable.ClassName))
            {
                dbTable.ClassName = dbTable.TableName;
            }
        }

        public static void AutoFillColumns(List<CodeColumns> dbColumns)
        {
            foreach (var item in dbColumns)
            {
                if (string.IsNullOrEmpty(item.ClassProperName))
                {
                    item.ClassProperName = item.DbColumnName;
                }
                if (string.IsNullOrEmpty(item.DbColumnName))
                {
                    item.DbColumnName = item.ClassProperName;
                }
            }
        }
    }
}
