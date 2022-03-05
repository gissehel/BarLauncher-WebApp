using FluentDataAccess;
using FluentDataAccess.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BarLauncher.WebApp.Lib.Core.Service;
using BarLauncher.WebApp.Lib.DomainModel;
using BarLauncher.WebApp.Lib.Service;
using BarLauncher.WebApp.Test.Mock.Service;

namespace BarLauncher.WebApp.Test.NUnit
{
    public class WebAppItemRepositoryTests
    {
        private ISystemWebAppService SystemWebAppService { get; set; }

        private IDataAccessService DataAccessService { get; set; }

        private IWebAppItemRepository WebAppItemRepository { get; set; }

        [SetUp]
        public void Setup()
        {
            SystemWebAppService = new SystemWebAppServiceMock
            {
                ApplicationDataPath = Helper.GetTestPath(),
                ApplicationName = "TestDatabase",
            };
            DataAccessService = DataAccessSQLite.GetService(SystemWebAppService);
            WebAppItemRepository = new WebAppItemRepository(DataAccessService);
        }

        [TearDown]
        public void TearDown()
        {
            if (DataAccessService != null)
            {
                DataAccessService.Dispose();
            }

            WebAppItemRepository = null;
            DataAccessService = null;
            SystemWebAppService = null;
        }

        public void Init()
        {
            DataAccessService.Init();
            WebAppItemRepository.Init();
        }

        private IEnumerable<WebAppItem> GetWebAppItems() => DataAccessService
                .GetQuery("select id, url, keywords, profile from webapp_item order by id;")
                .Returning<WebAppItem>()
                .Reading("id", (WebAppItem item, long value) => item.Id = value)
                .Reading("url", (WebAppItem item, string value) => item.Url = value)
                .Reading("keywords", (WebAppItem item, string value) => item.Keywords = value)
                .Reading("profile", (WebAppItem item, string value) => item.Profile = value)
                .Execute();

        private void EnsureSchema()
        {
            var schema = Helper.GetSchemaForTable(DataAccessService, "webapp_item");
            Assert.IsNotNull(schema);
            Assert.AreEqual("CREATE TABLE webapp_item (id integer primary key, url text, keywords text, search text, profile text)", schema);
        }

        private void CreateOldSchema()
        {
            DataAccessService.GetQuery("create table if not exists webapp_item (id integer primary key, url text, keywords text, search text);").Execute();
        }
        private void CreateNewSchema()
        {
            DataAccessService.GetQuery("create table if not exists webapp_item (id integer primary key, url text, keywords text, search text, profile text);").Execute();
        }

        [Test]
        public void UpgradeFromScratch()
        {
            Init();
            EnsureSchema();
            var items = GetWebAppItems();
            Assert.AreEqual(0, items.Count());
        }

        [Test]
        public void UpgradeFromOldVersionWithoutData()
        {
            DataAccessService.Init();
            CreateOldSchema();
            WebAppItemRepository.Init();
            EnsureSchema();
            var items = GetWebAppItems();
            Assert.AreEqual(0, items.Count());
        }

        [Test]
        public void UpgradeFromNewVersionWithoutData()
        {
            DataAccessService.Init();
            CreateNewSchema();
            WebAppItemRepository.Init();
            EnsureSchema();
            var items = GetWebAppItems();
            Assert.AreEqual(0, items.Count());
        }

        [Test]
        public void UpgradeFromOldVersionWithData()
        {
            DataAccessService.Init();
            CreateOldSchema();
            DataAccessService.GetQuery("insert into webapp_item values (1, 'https://url1.dom/x1', 'keywords1', 'search1');").Execute();
            WebAppItemRepository.Init();
            EnsureSchema();
            var items = GetWebAppItems();
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(1, items.First().Id);
            Assert.AreEqual("https://url1.dom/x1", items.First().Url);
            Assert.AreEqual("keywords1", items.First().Keywords);
            Assert.AreEqual("default", items.First().Profile);
        }

        [Test]
        public void UpgradeFromNewVersionWithData()
        {
            DataAccessService.Init();
            CreateNewSchema();
            DataAccessService.GetQuery("insert into webapp_item values (1, 'https://url1.dom/x1', 'keywords1', 'search1', 'mank');").Execute();
            WebAppItemRepository.Init();
            EnsureSchema();
            var items = GetWebAppItems();
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(1, items.First().Id);
            Assert.AreEqual("https://url1.dom/x1", items.First().Url);
            Assert.AreEqual("keywords1", items.First().Keywords);
            Assert.AreEqual("mank", items.First().Profile);
        }

        [Test]
        public void UpgradeFromOldVersionWithManyData()
        {
            DataAccessService.Init();
            CreateOldSchema();
            DataAccessService.GetQuery("insert into webapp_item values (1, 'https://url1.dom/x1', 'keywords1', 'search1');").Execute();
            DataAccessService.GetQuery("insert into webapp_item values (2, 'https://url2.dom/x2', 'keywords2', 'search2');").Execute();
            DataAccessService.GetQuery("insert into webapp_item values (3, 'https://url3.dom/x3', 'keywords3', 'search3');").Execute();
            WebAppItemRepository.Init();
            EnsureSchema();
            var items = GetWebAppItems();
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual(1, items.First().Id);
            Assert.AreEqual("https://url1.dom/x1", items.First().Url);
            Assert.AreEqual("keywords1", items.First().Keywords);
            Assert.AreEqual("default", items.First().Profile);
            Assert.AreEqual(2, items.ElementAt(1).Id);
            Assert.AreEqual("https://url2.dom/x2", items.ElementAt(1).Url);
            Assert.AreEqual("keywords2", items.ElementAt(1).Keywords);
            Assert.AreEqual("default", items.ElementAt(1).Profile);
            Assert.AreEqual(3, items.ElementAt(2).Id);
            Assert.AreEqual("https://url3.dom/x3", items.ElementAt(2).Url);
            Assert.AreEqual("keywords3", items.ElementAt(2).Keywords);
            Assert.AreEqual("default", items.ElementAt(2).Profile);
        }

        [Test]
        public void UpgradeFromNewVersionWithManyData()
        {
            DataAccessService.Init();
            CreateNewSchema();
            DataAccessService.GetQuery("insert into webapp_item values (1, 'https://url1.dom/x1', 'keywords1', 'search1', 'mank');").Execute();
            DataAccessService.GetQuery("insert into webapp_item values (2, 'https://url2.dom/x2', 'keywords2', 'search2', 'default');").Execute();
            DataAccessService.GetQuery("insert into webapp_item values (3, 'https://url3.dom/x3', 'keywords3', 'search3', 'shon');").Execute();
            WebAppItemRepository.Init();
            EnsureSchema();
            var items = GetWebAppItems();
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual(1, items.First().Id);
            Assert.AreEqual("https://url1.dom/x1", items.First().Url);
            Assert.AreEqual("keywords1", items.First().Keywords);
            Assert.AreEqual("mank", items.First().Profile);
            Assert.AreEqual(2, items.ElementAt(1).Id);
            Assert.AreEqual("https://url2.dom/x2", items.ElementAt(1).Url);
            Assert.AreEqual("keywords2", items.ElementAt(1).Keywords);
            Assert.AreEqual("default", items.ElementAt(1).Profile);
            Assert.AreEqual(3, items.ElementAt(2).Id);
            Assert.AreEqual("https://url3.dom/x3", items.ElementAt(2).Url);
            Assert.AreEqual("keywords3", items.ElementAt(2).Keywords);
            Assert.AreEqual("shon", items.ElementAt(2).Profile);
        }
    }
}