using FluentDataAccess;
using System.Collections.Generic;
using System.Linq;
using BarLauncher.WebApp.Lib.Core.Service;
using BarLauncher.WebApp.Lib.DomainModel;

namespace BarLauncher.WebApp.Lib.Service
{
    public class WebAppConfigurationRepository : IWebAppConfigurationRepository
    {
        private IDataAccessService DataAccessService { get; set; }

        public WebAppConfigurationRepository(IDataAccessService dataAccessService)
        {
            DataAccessService = dataAccessService;
        }

        private void UpgradeForProfile()
        {
            try
            {
                DataAccessService.GetQuery("select launcher from configuration").Execute();
                try
                {
                    DataAccessService.GetQuery("select profile from configuration").Execute();
                }
                catch (System.Exception)
                {
                    DataAccessService
                        .GetQuery(
                            "create temp table configuration_update (id integer primary key, profile text, launcher text, pattern text);" +
                            "insert into configuration_update (id, profile, launcher , pattern ) select id, 'default', launcher, pattern from configuration order by id limit 1;" +
                            "drop table configuration;" +
                            "create table if not exists configuration (id integer primary key, profile text, launcher text, pattern text);" +
                            "insert into configuration (id, profile, launcher , pattern ) select id, profile, launcher, pattern from configuration_update order by id;" +
                            "drop table configuration_update;"
                        )
                        .Execute();
                }
            }
            catch (System.Exception)
            {
                // No updagre needed
            }

        }

        public void Init()
        {
            UpgradeForProfile();
            DataAccessService
                .GetQuery("create table if not exists configuration (id integer primary key, profile text, launcher text, pattern text);")
                .Execute();
        }
        private string GetProfile(string profile) => profile ?? "default";

        public IEnumerable<WebAppConfiguration> GetConfigurations()
        {
            var configurations = DataAccessService
                .GetQuery("select profile, launcher, pattern from configuration;")
                .Returning<WebAppConfiguration>()
                .Reading("profile", (WebAppConfiguration configuration, string value) => configuration.Profile = value)
                .Reading("launcher", (WebAppConfiguration configuration, string value) => configuration.WebAppLauncher = value)
                .Reading("pattern", (WebAppConfiguration configuration, string value) => configuration.WebAppArgumentPattern = value)
                .Execute();
            if (configurations.Any())
            {
                return configurations;
            }
            return null;
        }

        public WebAppConfiguration GetConfiguration(string profile)
        {
            var configurations = DataAccessService
                .GetQuery("select profile, launcher, pattern from configuration where profile=@profile;")
                .WithParameter("profile", GetProfile(profile))
                .Returning<WebAppConfiguration>()
                .Reading("profile", (WebAppConfiguration configuration, string value) => configuration.Profile = value)
                .Reading("launcher", (WebAppConfiguration configuration, string value) => configuration.WebAppLauncher = value)
                .Reading("pattern", (WebAppConfiguration configuration, string value) => configuration.WebAppArgumentPattern = value)
                .Execute();
            return configurations.FirstOrDefault();
        }

        public void SaveConfiguration(WebAppConfiguration configuration)
        {
            var affectedRows = DataAccessService
                .GetQuery("update configuration set launcher=@launcher, pattern=@pattern where profile=@profile")
                .WithParameter("profile", configuration.Profile)
                .WithParameter("launcher", configuration.WebAppLauncher)
                .WithParameter("pattern", configuration.WebAppArgumentPattern)
                .Execute();
            if (affectedRows == 0)
            {
                DataAccessService
                    .GetQuery("insert into configuration (profile, launcher, pattern) values (@profile, @launcher, @pattern)")
                    .WithParameter("profile", configuration.Profile)
                    .WithParameter("launcher", configuration.WebAppLauncher)
                    .WithParameter("pattern", configuration.WebAppArgumentPattern)
                    .Execute();
            }
        }
    }
}