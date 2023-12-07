using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace DataSyncSdk
{
    public class OauthToken
    {
        public static string token = string.Empty;
        private static long expireTimeStamp = 0;

        /// <summary>
        /// 初始化Oauth，获取token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public static void InitOAuth2ClientCredentials(string clientId, string clientSecret)
        {
            if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret))
            {
                if (token == string.Empty && expireTimeStamp == 0)
                {
                    GetTokenAsync(clientId, clientSecret);
                }
                else if (expireTimeStamp - long.Parse(GetTimeStamp()) <= 600)
                {
                    GetTokenAsync(clientId, clientSecret);
                }
            }
            else
            {
                Console.WriteLine("config oauth client");
                return;
            }
        }


        /// <summary>
        /// 获取oauth token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        private static void GetTokenAsync(string clientId, string clientSecret)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(OAuth2Config.DefaultTimeOut);
                    var url = APIConfig.DefaultBaseUrl + "/oauth2/token";
                    var response = httpClient.PostAsync(url, new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("grant_type","client_credentials"),
                        new KeyValuePair<string, string>("client_id",clientId),
                        new KeyValuePair<string, string>("client_secret", clientSecret),
                    })).Result;
                    var str = response.Content.ReadAsStringAsync().Result;
                    var res = JObject.Parse(str);
                    expireTimeStamp = long.Parse(GetTimeStamp()) + 7200;
                    token = res["access_token"].ToString();
                    Console.WriteLine(token + "  expire:" + expireTimeStamp + "  now:" + GetTimeStamp());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
