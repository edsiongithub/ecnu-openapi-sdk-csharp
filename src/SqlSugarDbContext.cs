using SqlSugar;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace DataSyncSdk
{
    public class SqlSugarDbContext
    {
        public SqlSugarClient Db;
        public SqlSugarDbContext(string conStr, SqlSugar.DbType dbType) 
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = conStr,
                DbType = dbType,//DbType.SqlServer,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
            }); ;
            //调式代码 用来打印SQL 
          
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                    Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
            /**/
        }

    }
}
