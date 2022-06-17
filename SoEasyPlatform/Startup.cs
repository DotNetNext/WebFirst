using System;
namespace SoEasyPlatform
{
    /// <summary>
    /// 启动入口
    /// </summary>
    public class Startup
    {
        #region 配置参数
        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version = "1.4";
        /// <summary>
        /// 接口域名目录
        /// </summary>
        /// <param name="configuration"></param>
        public static string RootUrl = "/api/";

        /// <summary>
        /// CurrentDirectory
        /// </summary>
        public static string CurrentDirectory = Environment.CurrentDirectory;

        #endregion
    }
}
