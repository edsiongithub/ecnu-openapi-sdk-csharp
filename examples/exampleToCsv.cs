using DataSyncSdk;
public class exampleToCsv
{
    static void Main(string[] args)
    {
          //获取token所需要的配置
        OauthConfig.ClientId = "yourid";
        OauthConfig.ClientSecret = "yoursecret";
        //初始化token
        OauthToken.InitialOauthCredential(OauthConfig.ClientId, OauthConfig.ClientSecret);


        //api配置
        ApiConfig.ApiUrl = "/api/v1/sync/fakewithts";
        ApiConfig.OutputFilePath = @"D:\newsync.csv";
        ApiConfig.PageSize = 10;
        ApiConfig.ApiParameters.Add("ts", "0");

        //写入csv文件
        SdkApi.SyncToCsv<FakeData>(ApiConfig.OutputFilePath);
    }
}