using AllGreen.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Fixture
{
    public class Application_information_fixture : FixtureBase<WebAppContext>
    {
        public void Application_name_is(string name)
        {
            Context.ApplicationStarter.ApplicationInformationService.ApplicationName = name;
        }

        public void Application_verison_is(string version)
        {
            Context.ApplicationStarter.ApplicationInformationService.Version = version;
        }

        public void Application_url_is(string url)
        {
            Context.ApplicationStarter.ApplicationInformationService.HomepageUrl = url;
        }
    }
}
