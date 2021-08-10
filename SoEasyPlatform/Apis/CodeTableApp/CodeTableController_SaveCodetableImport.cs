using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoEasyPlatform.Apis
{
    /// <summary>
    /// 数据库导入虚拟类相关逻辑
    /// </summary>
    public partial class CodeTableController : BaseController
    {
        private string GetEntityType(List<CodeType> types, DbColumnInfo columnInfo, CodeTableController codeTableController)
        {
            var typeInfo = types.FirstOrDefault(y => y.DbType.Any(it => it.Name.Equals(columnInfo.DataType, StringComparison.OrdinalIgnoreCase)));
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
