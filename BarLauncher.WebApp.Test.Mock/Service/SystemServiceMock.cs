using BarLauncher.WebApp.Lib.Core.Service;
using BarLauncher.EasyHelper.Test.Mock.Service;
using System.IO;
using BarLauncher.EasyHelper.Core.Service;

namespace BarLauncher.WebApp.Test.Mock.Service
{
    public class DataAccessWebAppServiceMock : IDataAccessWebAppService
    {
        ISystemService SystemService { get; set; }
        public DataAccessWebAppServiceMock(ISystemService systemService)
        {
            SystemService = systemService;
        }
        public string DatabaseName => SystemService.ApplicationName;

        public string DatabasePath => SystemService.ApplicationDataPath;

        public string GetExportPath() => @".\ExportDirectory";

        public string GetUID() => "UID";
    }
}