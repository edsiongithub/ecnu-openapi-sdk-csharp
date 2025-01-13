using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;

namespace DataSyncSdk
{

    public static class OauthConfig
    {
        public static string ClientId { get; set; }
        public static string ClientSecret { get; set;}
        public static string DefaultScope { get; set; } = "ECNU";
        public static int DefaultTimeOut { get; set; } = 10;
    }
    public class ApiConfig
    {
        public ApiConfig(string apiUrl, int pageSize)
        {
            ApiUrl = apiUrl;
            PageSize = pageSize;
        }
        public ApiConfig()
        { }
        public  string DefaultBaseUrl { get; set; } = "https://api.ecnu.edu.cn";
        public  string OutputFilePath { get; set; } = "D:\\syncresult.csv";
        
        public string ApiUrl { get; set; }
        /// <summary>
        /// 取api数据时每页取的数据量，默认2000
        /// </summary>
        public  int PageSize { get; set; } = 2000;

        /// <summary>
        /// 分页写入数据库时每页数据量，默认10000
        /// 建议最大不超过100000，且设置为PageSize的整数倍
        /// </summary>
        public  int BatchSize { get; set; } = 10000;
        public  Dictionary<string, string> ApiParameters { get; set; } 
            = new Dictionary<string, string>();

        public void AddParameter(string key, string value)
        {
            //如果添加了重复的参数，新的值覆盖原有值
            if (this.ApiParameters.ContainsKey(key))
            {
                this.ApiParameters[key] = value;
            }
            else
            {
                this.ApiParameters.Add(key, value);
            }

        }

        public void DeleteParameter(string key, string value)
        {
            if (this.ApiParameters.ContainsKey(key))
            {
                this.ApiParameters.Remove(key);
            }
        }

        public string GenerateUrl(int pageNum, int pageSize)
        {
            string url = string.Format("{0}{1}?pageNum={2}&pageSize={3}", this.DefaultBaseUrl, this.ApiUrl, pageNum, pageSize);
            if (this.ApiParameters.Count == 0)
            {
                Console.WriteLine("no params");
                return url;
            }

            foreach (KeyValuePair<string, string> keyValuePair in this.ApiParameters)
            {
                if (!url.Contains("?"))
                    url += "?" + keyValuePair.Key + "=" + keyValuePair.Value;
                else
                    url += "&" + keyValuePair.Key + "=" + keyValuePair.Value;
            }
            return url;
        }
    }

}
