using FluentDataAccess;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.WebApp.Lib.Core.Service;
using BarLauncher.WebApp.Lib.DomainModel;

namespace BarLauncher.WebApp.Lib.Service
{
    public class WebAppService : IWebAppService
    {
        private IDataAccessService DataAccessService { get; set; }

        private IWebAppItemRepository WebAppItemRepository { get; set; }

        private IWebAppConfigurationRepository WebAppConfigurationRepository { get; set; }

        private ISystemService SystemService { get; set; }

        private IDataAccessWebAppService DataAccessWebAppService { get; set; }

        private IFileGeneratorService FileGeneratorService { get; set; }

        private IFileReaderService FileReaderService { get; set; }

        private IHelperService HelperService { get; set; }

        private IApplicationInformationService ApplicationInformationService { get; set; }

        public WebAppService(IDataAccessService dataAccessService, IWebAppItemRepository webAppItemRepository, IWebAppConfigurationRepository webAppConfigurationRepository, ISystemService systemService, IDataAccessWebAppService dataAccessWebAppService, IFileGeneratorService fileGeneratorService, IFileReaderService fileReaderService, IHelperService helperService, IApplicationInformationService applicationInformationService)
        {
            DataAccessService = dataAccessService;
            WebAppItemRepository = webAppItemRepository;
            WebAppConfigurationRepository = webAppConfigurationRepository;
            SystemService = systemService;
            DataAccessWebAppService = dataAccessWebAppService;
            FileGeneratorService = fileGeneratorService;
            FileReaderService = fileReaderService;
            HelperService = helperService;
            ApplicationInformationService = applicationInformationService;
        }

        public void Init()
        {
            DataAccessService.Init();
            WebAppItemRepository.Init();
            WebAppConfigurationRepository.Init();

            var configurations = WebAppConfigurationRepository.GetConfigurations();
            if (configurations == null)
            {
                var configuration = new WebAppConfiguration()
                {
                    Profile = "default",
                    WebAppLauncher = "chrome.exe",
                    WebAppArgumentPattern = "--app=\"{0}\" --profile-directory=\"Default\""
                };
                WebAppConfigurationRepository.SaveConfiguration(configuration);
            }
        }

        public void AddWebAppItem(string url, string keywords, string profile)
        {
            var item = new WebAppItem
            {
                Url = url,
                Keywords = keywords,
                Profile = profile,
            };
            WebAppItemRepository.AddItem(item);
        }

        public IEnumerable<WebAppItem> Search(IEnumerable<string> terms)
        {
            return WebAppItemRepository.SearchItems(terms);
        }

        public void UpdateLauncher(string launcher, string argumentPattern, string profile)
        {
            var configuration = WebAppConfigurationRepository.GetConfiguration(profile);
            if (configuration == null)
            {
                configuration = new WebAppConfiguration() 
                { 
                    Profile = profile,
                };
            }
            configuration.WebAppLauncher = launcher;
            configuration.WebAppArgumentPattern = argumentPattern;

            WebAppConfigurationRepository.SaveConfiguration(configuration);
        }

        public void StartUrl(string url, string profile)
        {
            var configuration = WebAppConfigurationRepository.GetConfiguration(profile);
            if (configuration != null)
            {
                var launcher = configuration.WebAppLauncher;
                var argumentsPattern = configuration.WebAppArgumentPattern;

                var arguments = string.Format(argumentsPattern, url);
                SystemService.StartCommandLine(launcher, arguments);
            }
        }

        public WebAppConfiguration GetConfiguration(string profile) => WebAppConfigurationRepository.GetConfiguration(profile);
        public WebAppConfiguration GetOrCreateConfiguration(string profile)
        {
            var configuration = WebAppConfigurationRepository.GetConfiguration(profile);
            if (configuration == null)
            {
                configuration = new WebAppConfiguration()
                {
                    Profile = profile,
                    WebAppLauncher = "chrome.exe",
                    WebAppArgumentPattern = "--app=\"{0}\" --profile-directory=\"Default\""
                };
                WebAppConfigurationRepository.SaveConfiguration(configuration);
            }
            return WebAppConfigurationRepository.GetConfiguration(profile);
        }

        public IEnumerable<string> GetProfiles() =>
            WebAppConfigurationRepository
            .GetConfigurations()
            .Select((configuration) => configuration.Profile);

        public Dictionary<string, WebAppConfiguration> GetConfigurations() =>
            WebAppConfigurationRepository
            .GetConfigurations()
            .GroupBy((WebAppConfiguration webAppConfiguration) => webAppConfiguration.Profile)
            .ToDictionary(g => g.Key, g => g.ToList().First());

        public void RemoveUrl(string url) => WebAppItemRepository.RemoveItem(url);

        public void Export()
        {
            var exportDirectory = DataAccessWebAppService.GetExportPath();
            var exportFilename = string.Format("{0}-Save-{1}.wap.txt", ApplicationInformationService.ApplicationName, DataAccessWebAppService.GetUID());
            var exportFullFilename = Path.Combine(exportDirectory, exportFilename);
            using (var fileGenerator = FileGeneratorService.CreateGenerator(exportFullFilename))
            {
                var configurations = GetConfigurations();
                if (configurations.ContainsKey("default"))
                {
                    var configuration = configurations["default"];
                    fileGenerator.AddLine(string.Format("# launcher: {0}", configuration.WebAppLauncher));
                    fileGenerator.AddLine(string.Format("# argumentsPattern: {0}", configuration.WebAppArgumentPattern));
                }
                foreach (var configuration in configurations.Values)
                {
                    if (configuration.Profile != "default")
                    {
                        fileGenerator.AddLine(string.Format("# launcher[{1}]: {0}", configuration.WebAppLauncher, configuration.Profile));
                        fileGenerator.AddLine(string.Format("# argumentsPattern[{1}]: {0}", configuration.WebAppArgumentPattern, configuration.Profile));
                    }
                }
                foreach (var webAppItem in WebAppItemRepository.SearchItems(new List<string>()))
                {
                    if (webAppItem.Profile == "default")
                    {
                        fileGenerator.AddLine(string.Format("{0} ({1})", webAppItem.Url, webAppItem.Keywords));
                    }
                    else
                    {
                        fileGenerator.AddLine(string.Format("{0} ({1}) [{2}]", webAppItem.Url, webAppItem.Keywords, webAppItem.Profile));
                    }
                    
                }
                fileGenerator.Generate();
            }
            SystemService.OpenDirectory(exportDirectory);
        }

        public bool FileExists(string path) => FileReaderService.FileExists(path);

        public void Import(string path)
        {
            using (var fileReader = FileReaderService.Read(path))
            {
                var line = fileReader.ReadLine();
                while (line != null)
                {
                    line = line.Trim(' ', '\t', '\r', '\n');
                    if (line.StartsWith("#"))
                    {
                        if (line.Contains(":"))
                        {
                            var indexOfSeperater = line.IndexOf(":");
                            var key = line.Substring(0, indexOfSeperater).Trim(' ', '\t', '\r', '\n');
                            var value = line.Substring(indexOfSeperater + 1, line.Length - indexOfSeperater - 1).Trim(' ', '\t', '\r', '\n');
                            string profile = null;
                            HelperService.ExtractProfile(key, ref key, ref profile);
                            key = key.TrimStart('#').Trim(' ', '\t', '\r', '\n');

                            var configuration = GetOrCreateConfiguration(profile);
                            var changed = false;
                            if (key == "launcher")
                            {
                                configuration.WebAppLauncher = value;
                                changed = true;
                            }
                            if (key == "argumentsPattern")
                            {
                                configuration.WebAppArgumentPattern = value;
                                changed = true;
                            }
                            if (changed)
                            {
                                WebAppConfigurationRepository.SaveConfiguration(configuration);
                            }
                        }
                    }
                    else
                    {
                        var elements = line.Split(' ');
                        var url = elements[0];
                        var keywords = string.Join(" ", elements.Skip(1).Where(e => e.Length > 0).ToArray());
                        keywords = keywords.TrimStart('(', ' ', '\t', '\r', '\n').TrimEnd(')', ' ', '\t', '\r', '\n');
                        string profile = null;
                        if (HelperService.ExtractProfile(keywords, ref keywords, ref profile))
                        {
                            keywords = keywords.TrimStart('(', ' ', '\t', '\r', '\n').TrimEnd(')', ' ', '\t', '\r', '\n');
                        }

                        if (!string.IsNullOrEmpty(url))
                        {
                            AddWebAppItem(url, keywords, profile);
                        }
                    }
                    line = fileReader.ReadLine();
                }
            }
        }

        public void Dispose()
        {
            DataAccessService.Dispose();
        }

        public WebAppItem GetUrlInfo(string url)
        {
            return WebAppItemRepository.GetItem(url);
        }

        public void EditWebAppItem(string url, string newUrl, string newKeywords, string newProfile)
        {
            WebAppItemRepository.EditWebAppItem(url, newUrl, newKeywords, newProfile);
        }
    }
}