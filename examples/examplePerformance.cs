using DataSyncSdk;
using System.Diagnostics;
public class examplePerformance
{
    static void Main(string[] args)
    {
        //获取token所需要的配置
        OAuth2Config.ClientId = "yourid";
        OAuth2Config.ClientSecret = "yoursecret";
        //初始化token
        SdkApi.InitOAuth2ClientCredentials(OAuth2Config.ClientId, OAuth2Config.ClientSecret);

        //api配置
        APIConfig.ApiUrl = "/api/v1/sync/fake";
        APIConfig.OutputFilePath = @"D:\newsync.csv";
        APIConfig.PageSize = 2000;
        APIConfig.BatchSize = 10000;
        APIConfig.ApiParameters.Add("totalNum", "1000000");

        //同步至模型
        Stopwatch st = new Stopwatch();
        st.Start();
        GC.Collect();
        var list = SdkApi.SyncToModel<FakeData>();
        double usedMemory = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;
        st.Stop();
        Console.WriteLine("消耗内存:" + usedMemory + "MB");
        Console.WriteLine("程序执行耗时：" + st.ElapsedMilliseconds / 1000 + "S");

        //同步至csv文件
        st.Start();
        GC.Collect();
        SdkApi.SyncToCsv<FakeData>(APIConfig.OutputFilePath);
        usedMemory = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;
        st.Stop();
        Console.WriteLine("消耗内存:" + usedMemory + "MB");
        Console.WriteLine("程序执行耗时：" + st.ElapsedMilliseconds / 1000 + "S");
        Console.ReadLine();



        //同步至数据库，数据库类型及链接字符串在SqlSugarDbContext类中配置
        st.Start();
        GC.Collect();
        string conStr = "DataSource=D:\\syncdb.db";
        SqlSugarDbContext db = new SqlSugarDbContext(conStr, SqlSugar.DbType.Sqlite);
        SdkApi.SyncToDb<FakeData>(db);
        usedMemory = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;
        st.Stop();
        Console.WriteLine("消耗内存:" + usedMemory + "MB");
        Console.WriteLine("程序执行耗时：" + st.ElapsedMilliseconds / 1000 + "S");
        Console.ReadLine();
    }
}