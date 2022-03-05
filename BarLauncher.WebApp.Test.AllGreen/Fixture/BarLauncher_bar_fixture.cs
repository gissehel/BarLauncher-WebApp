using AllGreen.Lib;
using System.Linq;
using BarLauncher.EasyHelper;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Fixture
{
    public class BarLauncher_bar_fixture : FixtureBase<WebAppContext>
    {
        public void Start_the_bar() => Context.ApplicationStarter.Start();

        public void Write_query(string query)
        {
            AssertDisplayed();

            Context.ApplicationStarter.BarLauncherContextService.SetQueryFromInterface(query);
        }

        public void Append__on_query(string element)
        {
            var newQuery = Context.ApplicationStarter.BarLauncherContextService.CurrentQuery + element;
            Write_query(newQuery);
        }

        private void AssertDisplayed()
        {
            if (!Context.ApplicationStarter.BarLauncherContextService.BarLauncherDisplayed)
            {
                throw new AllGreenException("The bar launcher application is not currently displayed. It should be");
            }
        }

        public void Display_bar_launcher()
        {
            Context.ApplicationStarter.BarLauncherContextService.BarLauncherDisplayed = true;
        }

        private BarLauncherResult AssertAndGetResult(int lineNumber)
        {
            var lineIndex = lineNumber - 1;
            var resultCount = Context.ApplicationStarter.BarLauncherContextService.Results.Count();
            if (lineIndex < 0 || lineIndex >= resultCount)
            {
                throw new AllGreenException(string.Format("Invalid line number [{0}], there are [{1}] result currently shown.", lineNumber, resultCount));
            }
            return Context.ApplicationStarter.BarLauncherContextService.Results.ElementAt(lineIndex);
        }

        public string The_title_of_result__is(int lineNumber) => AssertAndGetResult(lineNumber).Title;

        public string The_subtitle_of_result__is(int lineNumber) => AssertAndGetResult(lineNumber).SubTitle;

        public string The_number_of_results_is() => Context.ApplicationStarter.BarLauncherContextService.Results.Count().ToString();

        public void Select_line(int lineNumber)
        {
            AssertDisplayed();
            var lineIndex = lineNumber - 1;
            if (lineIndex < 0 || lineIndex >= Context.ApplicationStarter.BarLauncherContextService.Results.Count())
            {
                throw new AllGreenException(string.Format("Line number [{0}] is invalid, there is currently only [{1}] results shown", lineNumber, Context.ApplicationStarter.BarLauncherContextService.Results.Count()));
            }

            var result = Context.ApplicationStarter.BarLauncherContextService.Results.ElementAt(lineIndex);
            if (result.Action != null)
            {
                result.Action();
                Context.ApplicationStarter.BarLauncherContextService.BarLauncherDisplayed = !result.ShouldClose;
            }
        }

        public bool Bar_launcher_is_displayed()
        {
            return Context.ApplicationStarter.BarLauncherContextService.BarLauncherDisplayed;
        }

        public string The_current_query_is()
        {
            AssertDisplayed();
            return Context.ApplicationStarter.BarLauncherContextService.CurrentQuery;
        }
    }
}