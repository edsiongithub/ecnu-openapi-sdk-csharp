using DataSyncSdk;
public class exampleToModel
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
        ApiConfig.PageSize = 10;
        ApiConfig.ApiParameters.Add("ts", "0");

        //导入模型，可以根据业务再进行筛查
        List<FakeData> list = SdkApi.SyncToModel<FakeData>();
        var result = list.Where(x=> x.colInt1 > 30).ToList();
    }
}