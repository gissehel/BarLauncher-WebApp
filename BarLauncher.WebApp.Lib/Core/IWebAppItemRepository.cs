using System.Collections.Generic;
using BarLauncher.WebApp.Lib.DomainModel;

namespace BarLauncher.WebApp.Lib.Core.Service
{
    public interface IWebAppItemRepository
    {
        void Init();

        IEnumerable<WebAppItem> SearchItems(IEnumerable<string> terms);

        void AddItem(WebAppItem item);

        void RemoveItem(string url);

        WebAppItem GetItem(string url);

        void EditWebAppItem(string url, string newUrl, string newKeywords, string newProfile);
    }
}