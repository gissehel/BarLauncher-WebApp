using FluentDataAccess;
using System;
using System.IO;
using System.Reflection;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Test.Mock.Service;
using BarLauncher.WebApp.Lib.Core.Service;
using BarLauncher.WebApp.Lib.Service;
using BarLauncher.WebApp.Test.Mock.Service;

namespace BarLauncher.WebApp.Test.AllGreen.Helper
{
    public class ApplicationStarter
    {
        public BarLauncherContextServiceMock BarLauncherContextService { get; set; }
        public QueryServiceMock QueryService { get; set; }
        public SystemServiceMock SystemService { get; set; }
        public DataAccessWebAppServiceMock DataAccessWebAppService { get; set; }
        public IBarLauncherResultFinder BarLauncherWebAppResultFinder { get; set; }
        public IWebAppService WebAppService { get; set; }
        public IHelperService HelperService { get; set; }
        public FileGeneratorServiceMock FileGeneratorService { get; set; }
        public FileReaderServiceMock FileReaderService { get; set; }
        public ApplicationInformationServiceMock ApplicationInformationService { get; set; }
        private string TestName { get; set; }

        private string testPath = null;
        public string TestPath => testPath ?? (testPath = GetApplicationDataPath());

        public void Init(string testName)
        {
            TestName = testName;
            QueryServiceMock queryService = new QueryServiceMock();
            BarLauncherContextServiceMock barLauncherContextService = new BarLauncherContextServiceMock(queryService);
            SystemServiceMock systemService = new SystemServiceMock();
            DataAccessWebAppServiceMock dataAccessWebAppService = new DataAccessWebAppServiceMock(systemService);
            IDataAccessService dataAccessService = DataAccessSQLite.GetService(dataAccessWebAppService);
            IWebAppItemRepository webAppItemRepository = new WebAppItemRepository(dataAccessService);
            IWebAppConfigurationRepository webAppConfigurationRepository = new WebAppConfigurationRepository(dataAccessService);
            FileGeneratorServiceMock fileGeneratorService = new FileGeneratorServiceMock();
            FileReaderServiceMock fileReaderService = new FileReaderServiceMock();
            IHelperService helperService = new HelperService();
            ApplicationInformationServiceMock applicationInformationService = new ApplicationInformationServiceMock();
            IWebAppService webAppService = new WebAppService(dataAccessService, webAppItemRepository, webAppConfigurationRepository, systemService, dataAccessWebAppService, fileGeneratorService, fileReaderService, helperService, applicationInformationService);
            IBarLauncherResultFinder barLauncherWebAppResultFinder = new WebAppResultFinder(barLauncherContextService, webAppService, helperService, applicationInformationService, systemService);

            systemService.ApplicationDataPath = TestPath;

            BarLauncherContextService = barLauncherContextService;
            QueryService = queryService;
            SystemService = systemService;
            WebAppService = webAppService;
            FileGeneratorService = fileGeneratorService;
            FileReaderService = fileReaderService;
            BarLauncherWebAppResultFinder = barLauncherWebAppResultFinder;
            HelperService = helperService;
            ApplicationInformationService = applicationInformationService;

            BarLauncherContextService.AddQueryFetcher("wap", BarLauncherWebAppResultFinder);
        }

        public void Start()
        {
            BarLauncherWebAppResultFinder.Init();
        }

        private static string GetThisAssemblyDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var thisAssemblyCodeBase = assembly.Location;
            var thisAssemblyDirectory = Path.GetDirectoryName(new Uri(thisAssemblyCodeBase).LocalPath);

            return thisAssemblyDirectory;
        }

        private string GetApplicationDataPath()
        {
            var thisAssemblyDirectory = GetThisAssemblyDirectory();
            var path = Path.Combine(Path.Combine(thisAssemblyDirectory, "AllGreen"), string.Format("AG_{0:yyyyMMdd-HHmmss-fff}_{1}", DateTime.Now, TestName));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}