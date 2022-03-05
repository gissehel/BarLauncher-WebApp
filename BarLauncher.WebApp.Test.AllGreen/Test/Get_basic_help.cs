using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    public class Get_basic_help : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context_with_multiple_profiles>()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("wap"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("list [PATTERN] [PATTERN] [...]", "List all url matching patterns")
            .Check("config [PROFILE] [APP_PATH] [APP_ARGUMENT_PATTERN]", "Configure a new webapp launcher for a profile")
            .Check("add URL [KEYWORD] [KEYWORD] [...]", "Add a new url (or update an existing) with associated keywords")
            .Check("remove [URL|PATTERN]", "Remove an existing url")
            .Check("edit [URL|PATTERN] [ -> URL [KEYWORD] [KEYWORD] [...] [\\[PROFILE\\]]", "Edit an existing url")
            .Check("open URL", "Open an url as a web app without saving it")
            .Check("export", "Export urls to a file")
            .Check("import FILENAME", "Import urls from FILENAME")
            .Check("help", "BarLauncher-WebApp version 0.0 - (Go to BarLauncher-WebApp main web site)")
            .EndUsing()

             .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Append__on_query(" o"))
            .EndUsing()

             .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("config [PROFILE] [APP_PATH] [APP_ARGUMENT_PATTERN]", "Configure a new webapp launcher for a profile")
            .Check("remove [URL|PATTERN]", "Remove an existing url")
            .Check("open URL", "Open an url as a web app without saving it")
            .Check("export", "Export urls to a file")
            .Check("import FILENAME", "Import urls from FILENAME")
            .EndUsing()

          .EndTest();
    }
}