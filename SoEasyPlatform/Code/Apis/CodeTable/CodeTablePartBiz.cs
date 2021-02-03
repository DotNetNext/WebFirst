using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoEasyPlatform.Code.Apis
{
 
    public partial class CodeTableController : BaseController
    {
        private void SaveCodeTableToDb(CodeTableViewModel viewModel)
        {
            base.Check(string.IsNullOrEmpty(viewModel.TableName) || string.IsNullOrEmpty(viewModel.ClassName), "表名或者实体类名必须填一个");
            viewModel.ColumnInfoList = viewModel.ColumnInfoList
                .Where(it => !string.IsNullOrEmpty(it.ClassProperName) || !string.IsNullOrEmpty(it.DbColumnName)).ToList();
            base.Check(viewModel.ColumnInfoList.Count == 0, "请配置实体属性");
            var dbTable = mapper.Map<CodeTable>(viewModel);
            AutoFillTable(dbTable);
            var dbColumns = mapper.Map<List<CodeColumns>>(viewModel.ColumnInfoList);
            AutoFillColumns(dbColumns);
            if (viewModel.Id == null || viewModel.Id == 0)
            {
                CheckAddName(viewModel, CodeTableDb);
                var id = CodeTableDb.InsertReturnIdentity(dbTable);
                foreach (var item in dbColumns)
                {
                    item.CodeTableId = id;
                }
                CodeColumnsDb.InsertRange(dbColumns);
            }
            else
            {
                CheckUpdateName(viewModel, CodeTableDb);
                CodeTableDb.Update(dbTable);
                foreach (var item in dbColumns)
                {
                    item.CodeTableId = dbTable.Id;
                }

                var oldIds = CodeColumnsDb.GetList(it => it.CodeTableId == dbTable.Id).Select(it => it.Id).ToList();
                var delIds = oldIds.Where(it => !dbColumns.Select(y => y.Id).Contains(it)).ToList();
                CodeColumnsDb.DeleteByIds(delIds.Select(it => (object)it).ToArray());

                var updateColumns = dbColumns.Where(it => it.Id > 0).ToList();
                if (updateColumns.Count > 0)
                {
                    CodeColumnsDb.UpdateRange(updateColumns);
                }

                var insertColumns = dbColumns.Where(it => it.Id == 0).ToList();
                if (insertColumns.Count > 0)
                {
                    CodeColumnsDb.InsertRange(insertColumns);
                }

            }
        }
     
        private  void AutoFillTable(CodeTable dbTable)
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
        private void AutoFillColumns(List<CodeColumns> dbColumns)
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

        private void CheckAddName(CodeTableViewModel viewModel, Repository<CodeTable> codeTableDb)
        {
            CheckClassName(viewModel);
            var isAny = codeTableDb.IsAny(it => it.TableName == viewModel.TableName && it.IsDeleted == false);
            if (isAny) 
            {
                throw new Exception(viewModel.TableName + "表名已存在");
            }
        }
        private void CheckUpdateName(CodeTableViewModel viewModel, Repository<CodeTable> codeTableDb)
        {
            CheckClassName(viewModel);
            var isAny = codeTableDb.IsAny(it => it.TableName == viewModel.TableName && it.IsDeleted == false&&it.Id!=viewModel.Id);
            if (isAny)
            {
                throw new Exception(viewModel.TableName + "表名已存在");
            }
        }
        private  void CheckClassName(CodeTableViewModel viewModel)
        {
            var First = viewModel.ClassName.First().ToString();
            if (Regex.IsMatch(First, @"\d"))
            {
                new Exception(viewModel.ClassName + "不是有效类名");
            }
        }

        private string GetEntityType(List<CodeType> types, DbColumnInfo columnInfo, CodeTableController codeTableController)
        {
            var typeInfo= types.FirstOrDefault(y => y.DbType.Any(it => it.Name.Equals(columnInfo.DataType,StringComparison.OrdinalIgnoreCase)));
            if (typeInfo == null)
            {
                return "string100";
            }
            else 
            {
                return typeInfo.Name;
            }
        }
    }
}
