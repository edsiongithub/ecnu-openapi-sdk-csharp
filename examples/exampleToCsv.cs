using DataSyncSdk;
public class exampleToCsv
{
    static void Main(string[] args)
    {
          //获取token所需要的配置
        OAuth2Config.ClientId = "yourid";
        OAuth2Config.ClientSecret = "yoursecret";
        //初始化token
        OauthToken.InitOAuth2ClientCredentials(OAuth2Config.ClientId, OAuth2Config.ClientSecret);


        //api配置
        APIConfig.ApiUrl = "/api/v1/sync/fakewithts";
        APIConfig.OutputFilePath = @"D:\newsync.csv";
        APIConfig.PageSize = 10;
        APIConfig.ApiParameters.Add("ts", "0");

        //写入csv文件
        SdkApi.SyncToCsv<FakeData>(APIConfig.OutputFilePath);
    }
}