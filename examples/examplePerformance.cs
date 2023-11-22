using DataSyncSdk;
using System.Diagnostics;
public class examplePerformance
{
    static void Main(string[] args)
    {
           //获取token所需要的配置
           OauthConfig.ClientId = "yourid";
           OauthConfig.ClientSecret = "yoursecret";
           //初始化token
           OauthToken.InitialOauthCredential(OauthConfig.ClientId, OauthConfig.ClientSecret);

           //api配置
           ApiConfig.ApiUrl = "/api/v1/sync/fake";
           ApiConfig.OutputFilePath = @"D:\newsync.csv";
           ApiConfig.PageSize = 2000;
           ApiConfig.BatchSize = 10000;
           SdkApi.AddParameter("totalNum", "1000000");

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
           SdkApi.SyncToCsv<FakeData>(ApiConfig.OutputFilePath);
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