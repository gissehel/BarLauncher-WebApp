using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Service;
using BarLauncher.EasyHelper.Flow.Launcher;
using BarLauncher.WebApp.Lib.Service;
using FluentDataAccess;
using BarLauncher.EasyHelper.Flow.Launcher.Service;

namespace BarLauncher.WebApp.Flow.Launcher
{
    public class WebAppPlugin : FlowLauncherPlugin
    {
        public override IBarLauncherResultFinder PrepareContext()
        {
            var systemService = new FlowLauncherSystemService("BarLauncher-WebApp", BarLauncherContextService as BarLauncherContextService);
            var dataAccessWebAppService = new DataAccessWebAppService(systemService, "Wox.WebApp");
            var dataAccessService = DataAccessSQLite.GetService(dataAccessWebAppService);
            var helperService = new HelperService();
            var webAppConfigurationRepository = new WebAppConfigurationRepository(dataAccessService);
            var webAppItemRepository = new WebAppItemRepository(dataAccessService);
            var fileGeneratorService = new FileGeneratorService();
            var fileReaderService = new FileReaderService();
            var applicationInformationService = new ApplicationInformationService(systemService);
            var webAppService = new WebAppService(dataAccessService, webAppItemRepository, webAppConfigurationRepository, systemService, dataAccessWebAppService, fileGeneratorService, fileReaderService, helperService, applicationInformationService);
            return new WebAppResultFinder(BarLauncherContextService, webAppService, helperService, applicationInformationService, systemService);
        }
    }
}
