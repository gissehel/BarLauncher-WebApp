﻿using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Service;
using BarLauncher.EasyHelper.Wox;
using BarLauncher.WebApp.Lib.Service;
using FluentDataAccess;

namespace BarLauncher.WebApp.Wox
{
    public class WebAppPlugin : WoxPlugin
    {
        public override IBarLauncherResultFinder PrepareContext()
        {
            var systemWebAppService = new SystemWebAppService("BarLauncher-WebApp", "Wox.WebApp");
            var dataAccessService = DataAccessSQLite.GetService(systemWebAppService);
            var helperService = new HelperService();
            var webAppConfigurationRepository = new WebAppConfigurationRepository(dataAccessService);
            var webAppItemRepository = new WebAppItemRepository(dataAccessService);
            var fileGeneratorService = new FileGeneratorService();
            var fileReaderService = new FileReaderService();
            var applicationInformationService = new ApplicationInformationService(systemWebAppService);
            var webAppService = new WebAppService(dataAccessService, webAppItemRepository, webAppConfigurationRepository, systemWebAppService, fileGeneratorService, fileReaderService, helperService, applicationInformationService);
            return new WebAppResultFinder(BarLauncherContextService, webAppService, helperService, applicationInformationService, systemWebAppService);
        }
    }
}