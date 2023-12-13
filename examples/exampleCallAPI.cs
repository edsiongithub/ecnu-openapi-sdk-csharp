using DataSyncSdk;

namespace ecnu_openapi_sdk_csharp.examples
{
    public class exampleCallAPI
    {
        static void Main(string[] args)
        {
            //获取token所需要的配置
            OAuth2Config.ClientId = "yourid";
            OAuth2Config.ClientSecret = "yoursecret";
            //初始化token
            SdkApi.InitOAuth2ClientCredentials(OAuth2Config.ClientId, OAuth2Config.ClientSecret);

            //调用api，获取数据
            var res = SdkApi.CallApi("https://api.ecnu.edu.cn/api/v1/sync/fakewithts?ts=0&pageNum=1&pageSize=1", "get");
            Console.WriteLine(res);
        }
    }
}
