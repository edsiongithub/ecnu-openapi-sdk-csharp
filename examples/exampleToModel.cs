using DataSyncSdk;
public class exampleToModel
{
    static void Main(string[] args)
    {
        //获取token所需要的配置
        OAuth2Config.ClientId = "yourid";
        OAuth2Config.ClientSecret = "yoursecret";
        //初始化token
        SdkApi.InitOAuth2ClientCredentials(OAuth2Config.ClientId, OAuth2Config.ClientSecret);


        //api配置
        APIConfig.ApiUrl = "/api/v1/sync/fakewithts";
        APIConfig.PageSize = 10;
        APIConfig.ApiParameters.Add("ts", "0");

        //导入模型，可以根据业务再进行筛查
        List<FakeData> list = SdkApi.SyncToModel<FakeData>();
        var result = list.Where(x=> x.colInt1 > 30).ToList();
    }
}