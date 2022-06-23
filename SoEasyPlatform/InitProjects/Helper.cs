using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace SoEasyPlatform
{
    /// <summary>
    ///辅助方法
    /// </summary>
    public partial class InitTable
    {

        private void CheckConfigItemChilds(string configUrl, JToken item)
        {
            CheckJsonItem(item["模版"], "模版", item, configUrl);
            CheckJsonItem(item["文件夹"], "文件夹", item, configUrl);
            CheckJsonItem(item["子目录"], "子目录", item, configUrl);
            CheckJsonItem(item["文件后缀"], "文件后缀", item, configUrl);
            CheckJsonItem(item["描述"], "描述", item, configUrl);
        }

        private void CheckJsonItem(JToken jToken, string keyName, JToken item, string url)
        {
            if (jToken == null)
            {
                throw new System.Exception(url + "配置错误 ,缺少: " + keyName + "节点 。" + item.ToString());
            }
        }

        /// <summary>
        /// 验证是否存在项目配置
        /// </summary>
        /// <param name="configUrl"></param>
        /// <exception cref="System.Exception"></exception>
        private static void CheckProjectConfig(string configUrl)
        {
            if (FileSugar.IsExistFile(configUrl) == false)
            {
                throw new System.Exception("没有找到文件" + configUrl);
            }
        }
    }
}
