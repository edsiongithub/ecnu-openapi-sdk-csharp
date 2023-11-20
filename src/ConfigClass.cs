using System.Collections.Generic;
using System.Drawing.Printing;

namespace DataSyncSdk
{

    public static class OauthConfig
    {
        public static string ClientId { get; set; }
        public static string ClientSecret { get; set;}
        public static string DefaultScope { get; set; } = "ECNU";
        public static int DefaultTimeOut { get; set; } = 10;
    }
    public static class ApiConfig
    {
        public static string DefaultBaseUrl { get; set; } = "https://api.ecnu.edu.cn";
        public static string OutputFilePath { get; set; } = "D:\\syncresult.csv";
        public static string ApiUrl;
        /// <summary>
        /// 取api数据时每页取的数据量，默认2000
        /// </summary>
        public static int PageSize { get; set; } = 2000;

        /// <summary>
        /// 分页写入数据库时每页数据量，默认10000
        /// 建议最大不超过100000，且设置为PageSize的整数倍
        /// </summary>
        public static int BatchSize { get; set; } = 10000;
        public static Dictionary<string, string> ApiParameters { get; set; } 
            = new Dictionary<string, string>();
    }

}
