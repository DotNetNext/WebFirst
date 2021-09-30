using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class Table_ToExcel
    {
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dts"></param>
        /// <param name="name"></param>
        /// <param name="widths"></param>
        /// <returns></returns>
        public static byte[] ExportExcel(DataTable[] dts, string name, int[] widths = null)
        {
            XLWorkbook wb = new XLWorkbook();

            foreach (var dt in dts)
            {
                for (int i = 1; i < 15; i++)
                {
                    //删除Ignore列
                    if (dt.Columns.Contains("Column" + i))
                    {
                        dt.Columns.Remove("Column" + i);
                    }
                }
                var newdt = new DataTable();
                foreach (DataColumn item in dt.Columns)
                {
                    newdt.Columns.Add(item.ColumnName);
                }
                foreach (DataRow item in dt.Rows)
                {
                    DataRow dr = newdt.NewRow();
                    foreach (DataColumn c in dt.Columns)
                    {
                        var value = item[c.ColumnName] + "";
                        dr[c.ColumnName] = value;
                    }
                    newdt.Rows.Add(dr);
                }
                wb.Worksheets.Add(newdt, dt.TableName);
                var worksheet = wb.Worksheets.Last();
                foreach (var item in worksheet.Tables)
                {
                    item.Theme = XLTableTheme.None;
                }
                // 处理列
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                    //worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    //worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.Apricot;
                    //worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.White;
                    //worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.Black;
                }
                // 处理列宽
                var colsWidth = dt.Columns.Cast<DataColumn>().Select(it => 20).ToArray();
                if (widths != null)
                {
                    colsWidth = widths;
                }
                for (int j = 1; j <= colsWidth.Length; j++)
                {
                    worksheet.Columns(j, j).Width = colsWidth[j - 1];
                }

                //foreach (var item in worksheet.Cells())
                //{
                //    item.Style.Fill.BackgroundColor = XLColor.White;
                //    item.Style.Font.FontColor = XLColor.Black;
                //}
                // 缓存到内存流，然后返回
            }
            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                bytes = stream.ToArray();
            }
            return bytes;
        }

    }
}
