using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoEasyPlatform
{
    public class TreeModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonIgnoreAttribute]
        [JsonProperty(PropertyName = "isSelectable")]
        public bool? IsSelectable { get; set; }
        [JsonProperty(PropertyName = "subs")]
        public List<TreeModel> Subs { get; set; }
        [JsonIgnoreAttribute]
        public string ParentId { get; set; }
    }
}
