using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    public class Get_avanced_help : TestBase<WebAppContext>
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
            .DoAction(f => f.Write_query("wap hel"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("help", "BarLauncher-WebApp version 0.0 - (Go to BarLauncher-WebApp main web site)")
            .EndUsing()

            .UsingList<Url_opened_fixture>()
            .With<Url_opened_fixture.Result>(f => f.Url)
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<Url_opened_fixture>()
            .With<Url_opened_fixture.Result>(f => f.Url)
            .Check("https://github.com/gissehel/BarLauncher-WebApp")
            .EndUsing()

            .EndTest();
    }
}