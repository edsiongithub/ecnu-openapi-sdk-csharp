using DataSyncSdk;
public class exampleToDb
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

        /*
        "SqlServer": "Server = yourserver; Initial Catalog = yourdb; UID = youruser; PWD = yourpwd",
        "MySql": "server=yourserver;port=3306;user=youruser;password=yourpwd;database=yourdb;AllowLoadLocalInfile=true;",
        "Sqlite": "DataSource=D:\\syncdata.db;",
        "PgSql": "Host=yourserver;Port=35432;Database=yourdb;Username=youruser;Password=yourpwd;",
        "Oracle": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=yourserver)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=youruser;Password=yourpwd;Pooling='true';Max Pool Size=150"
        */
        //数据库连接根据自己的数据库类型来，创建DbContext时选对DbType
        string constr = "Server = yourserver; Initial Catalog = yourdb; UID = youruser; PWD = yourpwd";
        SqlSugarDbContext db = new SqlSugarDbContext(constr, SqlSugar.DbType.SqlServer);

        //导入Db，直接插入。适用于首次导入数据
        SdkApi.SyncToDb<FakeData>(db);
        //支持Merge模式写入。即表中数据存在，则进行update；数据不存在，则进行insert。此模式中，实体类需要设置主键[SugarColumn(IsPrimaryKey = true)]
        //SdkApi.SyncToDbMerge<FakeData>(db);
    }
}