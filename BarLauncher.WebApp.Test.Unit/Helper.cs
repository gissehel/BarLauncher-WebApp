using FluentDataAccess;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BarLauncher.WebApp.Test.Unit
{
    internal class ResultStruct
    {
        public string Data { get; set; }
    }

    internal class Helper
    {
        public static string TestName {get;set;}

        public static string GetSchemaForTable(IDataAccessService DataAccessService, string tableName)
        {
            var result = DataAccessService
                .GetQuery("select * from sqlite_master where tbl_name='"+tableName+"';")
                .Returning<ResultStruct>()
                .Reading("sql", (ResultStruct r, string value) => r.Data = value)
                .Execute()
                .FirstOrDefault();
            return result?.Data;
        }

        public static string GetTestPath()
        {
            string locationPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            string testPath = string.Format("Tests\\{0:yyyy-MM-dd_HH-mm-ss_fff}_{1}", DateTime.Now, TestName);
            string fullTestPath = Path.Combine(locationPath, testPath);
            Directory.CreateDirectory(fullTestPath);
            return fullTestPath;
        }
    }
}
