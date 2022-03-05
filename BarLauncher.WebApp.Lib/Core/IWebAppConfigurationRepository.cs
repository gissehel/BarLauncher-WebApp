using System.Collections.Generic;
using BarLauncher.WebApp.Lib.DomainModel;

namespace BarLauncher.WebApp.Lib.Core.Service
{
    public interface IWebAppConfigurationRepository
    {
        void Init();

        IEnumerable<WebAppConfiguration> GetConfigurations();

        WebAppConfiguration GetConfiguration(string profile);

        void SaveConfiguration(WebAppConfiguration configuration);
    }
}