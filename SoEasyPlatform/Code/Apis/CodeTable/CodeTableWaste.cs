using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public static void CheckAddName(CodeTableViewModel viewModel, Repository<CodeTable> codeTableDb)
        {
            CheckClassName(viewModel);
            var isAny = codeTableDb.IsAny(it => it.TableName == viewModel.TableName && it.IsDeleted == false);
            if (isAny) 
            {
                throw new Exception(viewModel.TableName + "表名已存在");
            }
        }
        public static void CheckUpdateName(CodeTableViewModel viewModel, Repository<CodeTable> codeTableDb)
        {
            CheckClassName(viewModel);
            var isAny = codeTableDb.IsAny(it => it.TableName == viewModel.TableName && it.IsDeleted == false&&it.Id!=viewModel.Id);
            if (isAny)
            {
                throw new Exception(viewModel.TableName + "表名已存在");
            }
        }
        private static void CheckClassName(CodeTableViewModel viewModel)
        {
            var First = viewModel.ClassName.First().ToString();
            if (Regex.IsMatch(First, @"\d"))
            {
                new Exception(viewModel.ClassName + "不是有效类名");
            }
        }

    }
}
