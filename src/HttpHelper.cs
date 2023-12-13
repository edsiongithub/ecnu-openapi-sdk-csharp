using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DataSyncSdk
{
    public class HttpHelper
    {
        static int retry = 0;
        public static string HttpGetAsync(string url)
        {
            string retString = string.Empty;

            if (!string.IsNullOrEmpty(SdkApi.token))
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(OAuth2Config.DefaultTimeOut);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SdkApi.token);
                    var response = httpClient.GetAsync(url).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        return "http client error";
                    }
                    else
                    {
                        Stream myResponseStream = response.Content.ReadAsStreamAsync().Result;
                        using (StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8")))
                        {
                            retString = myStreamReader.ReadToEnd();
                        }
                    }
                }
            }
            else
            {
                if (retry++ >= 3)
                    return "";
                SdkApi.InitOAuth2ClientCredentials(OAuth2Config.ClientId, OAuth2Config.ClientSecret);
                HttpGetAsync(url);
            }
            return retString;
        }

        public static string HttpRequest(string url, string method)
        {
            string res = string.Empty;
            switch (method.ToUpper())
            {
                case "GET":
                    res = "get result";
                    break;
                case "POST":
                    res = "post result";
                    break;
                case "PUT":
                    res = "put result";
                    break;
            }
            return res;
        }
    }
}
