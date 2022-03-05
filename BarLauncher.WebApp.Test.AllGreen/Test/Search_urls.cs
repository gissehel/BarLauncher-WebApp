using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    public class Search_urls : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()
            .IsRunnable()

            .Include<Prepare_common_context_with_multiple_profiles>()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("wap list"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine) [default]")
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing search engine) [pro]")
            .Check("Start https://stackoverflow.com/", "Start the url https://stackoverflow.com/ (questions answers) [default]")
            .Check("Start https://netflix.com/", "Start the url https://netflix.com/ (video) [default]")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("wap list gin"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine) [default]")
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing search engine) [pro]")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("wap gin"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine) [default]")
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing search engine) [pro]")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("wap video"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://netflix.com/", "Start the url https://netflix.com/ (video) [default]")
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check("chrome.exe", "--app=\"https://netflix.com/\" --profile-directory=\"Default\"")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap bing"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing search engine) [pro]")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check("chrome.exe", "--app=\"https://netflix.com/\" --profile-directory=\"Default\"")
            .Check("chrome.exe", "--app=\"https://bing.com/\" --profile-directory=\"Pro\"")
            .EndUsing()

            .EndTest();
    }
}