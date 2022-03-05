using BarLauncher.WebApp.Lib.Core.Service;
using BarLauncher.EasyHelper.Test.Mock.Service;
using System.IO;

namespace BarLauncher.WebApp.Test.Mock.Service
{
    public class SystemWebAppServiceMock : SystemServiceMock, ISystemWebAppService
    {
        public string DatabaseName => ApplicationName;

        public string DatabasePath => ApplicationDataPath;

        public string GetExportPath() => @".\ExportDirectory";

        public string GetUID() => "UID";
    }
}