using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarLauncher.WebApp.Lib.Core.Service;

namespace BarLauncher.WebApp.Test.Mock.Service
{
    public class ApplicationInformationServiceMock : IApplicationInformationService
    {
        public string ApplicationName { get; set; }

        public string Version { get; set; }

        public string HomepageUrl { get; set; }
    }
}
