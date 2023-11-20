# C# SDK

## 能力

- 授权模式（token 管理）
  - [x] client_credentials 模式
  - [ ] password 模式
  - [ ] authorization code 模式
- 接口调用
  - [x] GET 
  - [ ] POST
  - [ ] PUT
  - [ ] DELETE
- 数据同步（接口必须支持翻页）
  - 全量同步
    - [x] 同步为 csv 格式
    - [ ] 同步为 xls/xlsx 格式
    - [x] 同步到数据库
  - 增量同步（接口必须支持ts增量参数）
    - [x] 同步到数据库

## 依赖

- dotnet6.0 以上（C# 10.0)
- SqlSugar 5.1.4.109
- Newtonsoft.Json 13.0.3
- CsvHelper 30.0.1

## 相关资料

- [SqlSugar](https://www.donet5.com/Home/Doc)

## 支持的数据库

数据驱动由SqlSugar提供，理论上SqlSugar官网列出的数据库都可以支持，以下是测试的情况

| 数据库 | 驱动 | 测试情况 |
| --- | --- | --- |
| MySQL | https://www.donet5.com/Home/Doc | 测试通过 |
| SQLite | https://www.donet5.com/Home/Doc | 测试通过 |
| PostgreSQL | https://www.donet5.com/Home/Doc | 测试通过 |
| SQL Server | https://www.donet5.com/Home/Doc | 测试通过 |
| Oracle | https://www.donet5.com/Home/Doc | 测试通过 |

## 示例

skd已发布至Nuget中，用户可通过Nuget工具搜索Ecnu.OpenApi.Sdk进行安装，或使用如下脚本安装。

dotnet cli
```
dotnet add package Ecnu.OpenApi.Sdk --version 1.0.0
```
NuGet 包管理器命名
```
NuGet\Install-Package Ecnu.OpenApi.Sdk -Version 1.0.0
```

安装完成后，进行简单配置即可获得授权接口的数据。以下为简单示例
* 注意：开发者需要根据所需要数据的接口来编写实体类（如示例代码中的DnsRecord类需要根据dnsrecord接口数据自己编写）
```csharp
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
            //SdkApi.AddParameter("ts", "0");


            //初始化ouath，获取token
            OauthToken.InitialOauthCredential(OauthConfig.ClientId, OauthConfig.ClientSecret);


            //调用api，获取数据
            //var res = SdkApi.CallApi("https://api.ecnu.edu.cn/api/v1/sync/dnsrecord?pageSize=1000&pageNum=1&ts=0", "get");
            //Console.WriteLine(res);


            //写入csv文件
            //SdkApi.SyncToCsv<DnsRecord>(ApiConfig.OutputFilePath);

            //转换到Model
            //List<DnsRecord> list = SdkApi.SyncToModel<DnsRecord>();
            //Console.WriteLine(list.Count);
            //使用linq，在list中加入筛选逻辑
            //list.Where(x => x.deleted_mark == 0).ToList();


            //导入数据库，数据库类型在dbcontext中配置，用户需要根据所调用api返回的json内容创建模型类DnsRecord


            string constr = "Server = localhost; Initial Catalog = yourdb; Integrated Security = true";
            SqlSugarDbContext db = new SqlSugarDbContext(constr, SqlSugar.DbType.SqlServer);



            //SdkApi.SyncToDb<FakeData>(db);
            Console.WriteLine("==================================================================");

       
            SdkApi.SyncToDb<FakeData>(db);
            Console.ReadLine();
        }
```


## 性能

性能与 ORM 的实现方式（特别是对 upsert 的实现方式），数据库的实现方式，以及网络环境有关，不一定适用于所有情况。

当同步到数据库时，SDK 会采用分批读取/写入的方式，以减少内存的占用。

当同步到模型时，则会将所有数据写入到一个数组中，可能会占用较大的内存。

以下是测试环境

### 同步程序运行环境
 - i7 cpu
 - 16G 内存
 - windows11 X64
 - WD 固态硬盘
 - dotnet 6.0

### 数据库运行环境
mysql/postgresql/sqlserver all in one
 - 2 cpu
 - 8G 内存
 - anolis8 amd64
 - ESSD 云盘 PL0

  
### 测试接口信息
 - /api/v1/sync/fake
 - 使用 pageSize=2000 仅限同步
 - 接口请求耗时约 0.1 - 0.2 秒
 - 接口数据示例

```json
{
	"errCode": 0,
	"errMsg": "success",
	"requestId": "73a60094-c0f1-4daf-bc58-4626fbef7a2b",
	"data": {
		"pageSize": 2000,
		"pageNum": 1,
		"totalNum": 10000,
		"rows": [{
			"id": 1,
			"colString1": "Oxqmn5MWCt",
			"colString2": "mzavQncWeNlOlFgUW7HC",
			"colString3": "mvy6K1HU7rdCicPbvvA3rNZcDWPhvV",
			"colString4": "XGsK5NVQHOu4JrmHZ9ZL1iLf0UYpdIvNIzswULzb",
			"colInt1": 3931594532918648027,
			"colInt2": 337586114254574578,
			"colInt3": 2291922259603323213,
			"colInt4": 3000562485500051124,
			"colFloat1": 0.46541339000557547,
			"colFloat2": 0.6307996439929248,
			"colFloat3": 0.9278393850101392,
			"colFloat4": 0.7286866920659677,
			"colSqlTime1": "2023-10-20 22:02:07",
			"colSqlTime2": "2023-10-20 22:02:07",
			"colSqlTime3": "2023-10-20 22:02:07",
			"colSqlTime4": "2023-10-20 22:02:07"
		}]
	}
}
```

### 10000 数据量
#### 同步到模型

- 耗时：2 秒
- 内存分配：84.6M

#### 同步到数据库

含模型同步时间

|数据库|服务版本|全量写入耗时|全量更新耗时|内存分配|
|--|--|--|--|--|
|MySQL|8.0.34|4秒|4秒|105M|
|PostgreSQL|10.23|3秒|3秒|91M|
|SQLServer|16.0|4秒|4秒|85M|
|SQLite|/|3秒|3秒|85M|

### 100000 数据量
#### 同步到模型

- 耗时：19 秒
- 内存分配：154M

#### 同步到数据库

含模型同步时间

|数据库|服务版本|全量写入耗时|全量更新耗时|内存分配|
|--|--|--|--|--|
|MySQL|8.0.34|28秒|28秒|116M|
|PostgreSQL|10.23|28秒|24秒|97M|
|SQLServer|16.0|20秒|24秒|97M|
|SQLite|/|24秒|9.02秒|89M|

### 1000000 数据量
#### 同步到模型

- 耗时：195 秒
- 内存分配：967M

#### 同步到数据库

含模型同步时间

|数据库|服务版本|全量写入耗时|全量更新耗时|内存分配|
|--|--|--|--|--|
|MySQL|8.0.34|295秒|285秒|128M|
|PostgreSQL|10.23|370秒|362秒|100M|
|SQLServer|16.0|202秒|210秒|92M|
|SQLite|/|185秒|198秒|96M|
