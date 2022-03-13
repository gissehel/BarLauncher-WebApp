using FluentDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using BarLauncher.WebApp.Lib.Core.Service;
using BarLauncher.WebApp.Lib.DomainModel;
using BarLauncher.WebApp.Lib.Service;
using BarLauncher.WebApp.Test.Mock.Service;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Test.Mock.Service;
using Xunit;

namespace BarLauncher.WebApp.Test.Unit
{

    [SetContext]
    public class WebAppConfigurationRepositoryTests 
    {
        private ISystemService SystemService { get; set; }
        private IDataAccessWebAppService DataAccessWebAppService { get; set; }

        private IDataAccessService DataAccessService { get; set; }

        private IWebAppConfigurationRepository WebAppConfigurationRepository { get; set; }


        private void SetUp()
        {
            SystemService = new SystemServiceMock
            {
                ApplicationDataPath = Helper.GetTestPath(),
                ApplicationName = "TestDatabase",
            };
            DataAccessWebAppService = new DataAccessWebAppServiceMock(SystemService);
            DataAccessService = DataAccessSQLite.GetService(DataAccessWebAppService);
            WebAppConfigurationRepository = new WebAppConfigurationRepository(DataAccessService);
        }

        private void TearDown()
        {
            if (DataAccessService != null)
            {
                DataAccessService.Dispose();
            }

            WebAppConfigurationRepository = null;
            DataAccessService = null;
            SystemService = null;
        }

        private void Init()
        {
            DataAccessService.Init();
            WebAppConfigurationRepository.Init();
        }

        private class ResultStruct
        {
            public string Data { get; set; }
        }

        private IEnumerable<WebAppConfiguration> GetWebAppConfigurations() => DataAccessService
                .GetQuery("select profile, launcher, pattern from configuration;")
                .Returning<WebAppConfiguration>()
                .Reading("profile", (WebAppConfiguration configuration, string value) => configuration.Profile = value)
                .Reading("launcher", (WebAppConfiguration configuration, string value) => configuration.WebAppLauncher = value)
                .Reading("pattern", (WebAppConfiguration configuration, string value) => configuration.WebAppArgumentPattern = value)
                .Execute();

        private void EnsureSchema()
        {
            var schema = Helper.GetSchemaForTable(DataAccessService, "configuration");
            Assert.NotNull(schema);
            Assert.Equal("CREATE TABLE configuration (id integer primary key, profile text, launcher text, pattern text)", schema);
        }

        private void CreateOldSchema()
        {
            DataAccessService.GetQuery("create table if not exists configuration (id integer primary key, launcher text, pattern text);").Execute();
        }

        private void CreateNewSchema()
        {
            DataAccessService.GetQuery("create table if not exists configuration (id integer primary key, profile text, launcher text, pattern text);").Execute();
        }

        [Fact]
        public void UpgradeFromScratch()
        {
            SetUp();
            Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.Empty(configurations);
            TearDown();
        }

        [Fact]
        public void UpgradeFromOldVersionWithoutData()
        {
            SetUp();
            DataAccessService.Init();
            CreateOldSchema();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.Empty(configurations);
            TearDown();
        }

        [Fact]
        public void UpgradeFromNewVersionWithoutData()
        {
            SetUp();
            DataAccessService.Init();
            CreateNewSchema();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.Empty(configurations);
            TearDown();
        }

        [Fact]
        public void UpgradeFromOldVersionWithData()
        {
            SetUp();
            DataAccessService.Init();
            CreateOldSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'launcher', 'args');").Execute();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.Single(configurations);
            Assert.Equal("default", configurations.First().Profile);
            Assert.Equal("launcher", configurations.First().WebAppLauncher);
            Assert.Equal("args", configurations.First().WebAppArgumentPattern);
            TearDown();
        }

        [Fact]
        public void UpgradeFromNewVersionWithData()
        {
            SetUp();
            DataAccessService.Init();
            CreateNewSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'default', 'launcher', 'args');").Execute();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.Single(configurations);
            Assert.Equal("default", configurations.First().Profile);
            Assert.Equal("launcher", configurations.First().WebAppLauncher);
            Assert.Equal("args", configurations.First().WebAppArgumentPattern);
            TearDown();
        }

        [Fact]
        public void UpgradeFromOldVersionWithManyData()
        {
            SetUp();
            DataAccessService.Init();
            CreateOldSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'launcher', 'args');").Execute();
            DataAccessService.GetQuery("insert into configuration values (2, 'launcher2', 'args2');").Execute();
            DataAccessService.GetQuery("insert into configuration values (3, 'launcher3', 'args2');").Execute();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.Single(configurations);
            Assert.Equal("default", configurations.First().Profile);
            Assert.Equal("launcher", configurations.First().WebAppLauncher);
            Assert.Equal("args", configurations.First().WebAppArgumentPattern);
            TearDown();
        }

        [Fact]
        public void UpgradeFromNewVersionWithManyData()
        {
            SetUp();
            DataAccessService.Init();
            CreateNewSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'default', 'launcher', 'args');").Execute();
            DataAccessService.GetQuery("insert into configuration values (2, 'profile2', 'launcher2', 'args2');").Execute();
            DataAccessService.GetQuery("insert into configuration values (3, 'profile3', 'launcher3', 'args3');").Execute();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.Equal(3, configurations.Count());
            Assert.Equal("default", configurations.First().Profile);
            Assert.Equal("launcher", configurations.First().WebAppLauncher);
            Assert.Equal("args", configurations.First().WebAppArgumentPattern);
            Assert.Equal("profile2", configurations.ElementAt(1).Profile);
            Assert.Equal("launcher2", configurations.ElementAt(1).WebAppLauncher);
            Assert.Equal("args2", configurations.ElementAt(1).WebAppArgumentPattern);
            Assert.Equal("profile3", configurations.ElementAt(2).Profile);
            Assert.Equal("launcher3", configurations.ElementAt(2).WebAppLauncher);
            Assert.Equal("args3", configurations.ElementAt(2).WebAppArgumentPattern);
            TearDown();
        }
    }
}