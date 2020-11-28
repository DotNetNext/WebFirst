using System;
using System.Collections.Generic;
using System.Text;

namespace SugarSite.Enties
{
    public class ApiResult<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsKeyValuePair { get; set; }
        public string Url { get; set; }
    }
}
