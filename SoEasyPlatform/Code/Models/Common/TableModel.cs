using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace SoEasyPlatform
{
    public class TableModel<T>
    {
        
        [JsonProperty(PropertyName = "columns")]
        public List<TableColumn> Columns
        {
            get
            {
                List<TableColumn> columnList = new List<TableColumn>();
                columnList.Add(new TableColumn() { Checkbox = true });
                var properties = typeof(T).GetProperties();
                foreach (var item in properties)
                {
                    TableColumn column = new TableColumn();
                    column.Field = item.Name;
                    column.Title = item.Name;
                    var attrs = item.GetCustomAttributes(true);
                    var attr = attrs.FirstOrDefault(it => it is DisplayName);
                    if (attr != null)
                    {
                        column.Title = (attr as DisplayName).Name;
                    }
                    columnList.Add(column);
                }
                return columnList;
            }
        }
        [JsonProperty(PropertyName = "data")]
        public List<T> Rows { get; set; }
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
        [JsonProperty(PropertyName = "pagination")]
        public bool Pagination => true;
        [JsonProperty(PropertyName = "pageList")]
        public int[] PageList => new int[] { 10, 25, 50, 100 };
        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }
        [JsonProperty(PropertyName = "pageNumber")]
        public int PageNumber { get; set; }
    }

    public class TableColumn
    {
        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "checkbox")]
        public bool Checkbox { get; set; }

    }
}
