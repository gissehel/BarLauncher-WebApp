using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.WebApp.Lib.Core.Service;

namespace BarLauncher.WebApp.Lib.Service
{
    public class ApplicationInformationService : IApplicationInformationService
    {
        private ISystemService SystemService { get; set; }

        public ApplicationInformationService(ISystemService systemService)
        {
            SystemService = systemService;
        }

        public string ApplicationName => SystemService.ApplicationName;

        public string Version => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        public string HomepageUrl => "https://github.com/gissehel/BarLauncher-WebApp";
    }
}
