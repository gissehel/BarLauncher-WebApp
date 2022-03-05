using NUnit.Framework;
using BarLauncher.WebApp.Lib.Core.Service;
using BarLauncher.WebApp.Lib.Service;

namespace BarLauncher.WebApp.Test.NUnit
{
    public class HelperServiceTests
    {
        private IHelperService HelperService { get; set; }

        [SetUp]
        public void Setup()
        {
            HelperService = new HelperService();
        }

        [TearDown]
        public void TearDown()
        {
            HelperService = null;
        }

        [Test]
        public void ExtractNothingToExtract()
        {
            var keywords = "poide praf pido";
            string profile = null;
            Assert.False(HelperService.ExtractProfile(keywords, ref keywords, ref profile));
            Assert.AreEqual("poide praf pido", keywords);
            Assert.Null(profile);
        }
        [Test]
        public void ExtractValue()
        {
            var keywords = "poide praf pido [profile]";
            string profile = null;
            Assert.True(HelperService.ExtractProfile(keywords, ref keywords, ref profile));
            Assert.AreEqual("poide praf pido", keywords);
            Assert.AreEqual("profile", profile);
        }
        [Test]
        public void ExtractEmptyString()
        {
            var keywords = "poide praf pido []";
            string profile = null;
            Assert.False(HelperService.ExtractProfile(keywords, ref keywords, ref profile));
            Assert.AreEqual("poide praf pido []", keywords);
            Assert.Null(profile);
        }
        [Test]
        public void ExtractBogusString()
        {
            var keywords = "poide praf pido [poide";
            string profile = null;
            Assert.False(HelperService.ExtractProfile(keywords, ref keywords, ref profile));
            Assert.AreEqual("poide praf pido [poide", keywords);
            Assert.Null(profile);
        }
    }
}
