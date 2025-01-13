using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataSyncSdk
{
    public class SdkApi
    {
        public static void SyncToDb()
        {

        }

        #region 对外暴露的公有方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string CallApi(string url, string method)
        {
            string str = string.Empty;
            if (method.ToLower() == "get")
            {
                str = HttpHelper.HttpGetAsync(url);
            }
            return str;
        }


        /// <summary>
        /// json写入csv文件
        /// </summary>
        /// <param name="path"></param>
        public static void SyncToCsv<T>(string path, ApiConfig api) where T : class
        {
            if (OauthConfig.ClientId != null && OauthConfig.ClientSecret != null && api.ApiUrl != null
                && api.ApiParameters != null && api.OutputFilePath != null)
            {
                var expandos = GetAllRows<T>(api);
                if (expandos == null)
                {
                    Console.WriteLine("bad request,check api configs");
                    return;
                }
                using (TextWriter writer = new StreamWriter(path))
                {
                    using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        csv.WriteRecords((expandos as IEnumerable<dynamic>));
                    }
                }
            }
            else
            {
                Console.WriteLine("请配置api参数");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<T> SyncToModel<T>(ApiConfig api) where T : class
        {
            List<T> list = new List<T>();
            if (OauthConfig.ClientId != null && OauthConfig.ClientSecret != null && api.ApiUrl != null
                && api.ApiParameters != null && api.OutputFilePath != null)
            {
                list = GetAllRows<T>(api);
                if (list == null)
                {
                    Console.WriteLine("bad request,check api configs");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("please check ConfigClass...");
                return null;
            }
            return list;
        }

        /// <summary>
        /// 同步至数据库，插入
        /// </summary>
        /// <param name="db">dbcontext</param>
        /// <typeparam name="T"></typeparam>
        public static void SyncToDb<T>(SqlSugarDbContext db, ApiConfig api) where T : class, new()
        {
            int bulckPageSize = api.BatchSize;
            if (OauthConfig.ClientId != null && OauthConfig.ClientSecret != null && api.ApiUrl != null
                && api.ApiParameters != null && api.OutputFilePath != null)
            {
                db.Db.CodeFirst.InitTables(typeof(T));
                int pageNum;
                int sum = 0;
                List<T> allRows = new List<T>();
                for (pageNum = 1; pageNum < int.MaxValue; pageNum++)
                {
                    var row = GetRows<T>(pageNum, api);
                    if (row.Count != 0)
                    {
                        allRows.AddRange(row);
                        sum += row.Count;
                        if (sum == bulckPageSize)
                        {
                            db.Db.Fastest<T>().PageSize(bulckPageSize).BulkCopy(allRows);
                            sum = 0;
                            GC.Collect();
                            allRows = new List<T>();
                        }
                    }
                    else
                        break;
                }
                if (sum != 0)
                {
                    db.Db.Fastest<T>().PageSize(sum).BulkCopy(allRows);
                }
            }
            else
            {
                Console.WriteLine("请配置api参数");
            }
        }


        /// <summary>
        /// Merge模式允许多次同步至数据库
        /// 写入数据库时会根据主键判断数据是否存在，存在则更新，不存在则插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        public static void SyncToDbMerge<T>(SqlSugarDbContext db, ApiConfig api) where T : class, new()
        {
            int bulckPageSize = api.BatchSize;
            if (OauthConfig.ClientId != null && OauthConfig.ClientSecret != null && api.ApiUrl != null
                && api.ApiParameters != null && api.OutputFilePath != null)
            {
                db.Db.CodeFirst.InitTables(typeof(T));
                int pageNum;
                int sum = 0;
                List<T> allRows = new List<T>();
                for (pageNum = 1; pageNum < int.MaxValue; pageNum++)
                {
                    var row = GetRows<T>(pageNum, api);
                    if (row.Count != 0)
                    {
                        allRows.AddRange(row);
                        sum += row.Count;
                        if (sum == bulckPageSize)
                        {
                            db.Db.Fastest<T>().PageSize(bulckPageSize).BulkMerge(allRows);
                            sum = 0;
                            GC.Collect();
                            allRows = new List<T>();
                        }
                    }
                    else
                        break;
                }
                if (sum != 0)
                {
                    db.Db.Fastest<T>().PageSize(sum).BulkMerge(allRows);
                }
            }
            else
            {
                Console.WriteLine("请配置api参数");
            }
        }
     
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取api数据，解析出纯数据行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private static List<T> GetRows<T>(int pageNum, ApiConfig api) where T : class
        {
            string url = api.GenerateUrl(pageNum, api.PageSize);
            string res = HttpHelper.HttpGetAsync(url);
            if (res.Contains("error"))
                return null;
            var data = JObject.Parse(res);
            string rowsJson = data["data"]["rows"].ToString();
            List<T> rows = DeserializeJsonToList<T>(rowsJson);
            return rows;
        }

        /// <summary>
        /// 实现翻页，一页一页的解析数据行。最后返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> GetAllRows<T>(ApiConfig api) where T : class
        {
            List<T> allRows = new List<T>();
            int pageNum;
            for (pageNum = 1; pageNum < int.MaxValue; pageNum++)
            {
                var row = GetRows<T>(pageNum, api);
                if (row == null)
                    return null;
                if (row.Count != 0)
                {
                    allRows.AddRange(row);
                }
                else
                    break;
            }
            return allRows;
        }

        /// <summary>
        /// josn 转 List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        private static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object obj = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = obj as List<T>;
            return list;
        }
        #endregion

    }
}
