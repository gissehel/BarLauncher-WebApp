using BarLauncher.WebApp.Lib.Core.Service;
using BarLauncher.WebApp.Lib.Service;
using Xunit;

namespace BarLauncher.WebApp.Test.Unit
{
    public class HelperServiceTests
    {
        private IHelperService HelperService { get; set; }

        public HelperServiceTests()
        {
            HelperService = new HelperService();
        }

        [Fact]
        public void ExtractNothingToExtract()
        {
            var keywords = "poide praf pido";
            string profile = null;
            Assert.False(HelperService.ExtractProfile(keywords, ref keywords, ref profile));
            Assert.Equal("poide praf pido", keywords);
            Assert.Null(profile);
        }

        [Fact]
        public void ExtractValue()
        {
            var keywords = "poide praf pido [profile]";
            string profile = null;
            Assert.True(HelperService.ExtractProfile(keywords, ref keywords, ref profile));
            Assert.Equal("poide praf pido", keywords);
            Assert.Equal("profile", profile);
        }

        [Fact]
        public void ExtractEmptyString()
        {
            var keywords = "poide praf pido []";
            string profile = null;
            Assert.False(HelperService.ExtractProfile(keywords, ref keywords, ref profile));
            Assert.Equal("poide praf pido []", keywords);
            Assert.Null(profile);
        }

        [Fact]
        public void ExtractBogusString()
        {
            var keywords = "poide praf pido [poide";
            string profile = null;
            Assert.False(HelperService.ExtractProfile(keywords, ref keywords, ref profile));
            Assert.Equal("poide praf pido [poide", keywords);
            Assert.Null(profile);
        }

        [Fact]
        public void ExtractOnlyProfile()
        {
            var keywords = "[poide]";
            string profile = null;
            Assert.True(HelperService.ExtractProfile(keywords, ref keywords, ref profile));
            Assert.Equal("", keywords);
            Assert.Equal("poide", profile);
        }
    }
}
