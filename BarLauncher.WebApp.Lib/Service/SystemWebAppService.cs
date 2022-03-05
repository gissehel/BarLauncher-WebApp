using System;
using System.IO;
using System.Linq;
using BarLauncher.EasyHelper.Service;
using BarLauncher.WebApp.Lib.Core.Service;

namespace BarLauncher.WebApp.Lib.Service
{
    public class SystemWebAppService : SystemService, ISystemWebAppService
    {

        public SystemWebAppService(string applicationName) : base(applicationName)
        {
        }

        private string GetDatabaseName(string applicationDataPath, string applicationName) => Path.Combine(applicationDataPath, applicationName + ".sqlite");

        public SystemWebAppService(string applicationName, params string[] oldApplicationNames) : base(applicationName)
        {
            var currentDatabaseName = GetDatabaseName(ApplicationDataPath, ApplicationName);

            if (! File.Exists(currentDatabaseName))
            {
                foreach (var oldApplicationName in oldApplicationNames.AsEnumerable().Reverse())
                {
                    string oldDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), oldApplicationName);

                    var oldDatabaseName = GetDatabaseName(oldDataPath, oldApplicationName);
                    if (File.Exists(oldDatabaseName))
                    {
                        File.Move(oldDatabaseName, currentDatabaseName);
                        return;
                    }
                }
            }
        }

        public string DatabasePath => this.ApplicationDataPath;

        public string GetExportPath() => ApplicationDataPath;

        public string GetUID() => string.Format("{0:yyyyMMdd-HHmmss-fff}", DateTime.Now);
    }
}