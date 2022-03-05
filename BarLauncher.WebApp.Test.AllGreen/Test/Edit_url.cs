using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    internal class Edit_url : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()
            .IsRunnable()
            .Include<Prepare_common_context>()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("wap edi"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "edit [URL|PATTERN] [ -> URL [KEYWORD] [KEYWORD] [...] [\\[PROFILE\\]]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Edit an existing url")
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())

            .DoCheck(f => f.The_current_query_is(), "wap edit ")
            .DoCheck(f => f.The_number_of_results_is(), "4")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Edit https://google.com/", "Edit the url https://google.com/ (google search engine) [default]")
            .Check("Edit https://bing.com/", "Edit the url https://bing.com/ (bing search engine) [default]")
            .Check("Edit https://stackoverflow.com/", "Edit the url https://stackoverflow.com/ (questions answers) [default]")
            .Check("Edit https://netflix.com/", "Edit the url https://netflix.com/ (video) [default]")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Append__on_query("bin"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Edit https://bing.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Edit the url https://bing.com/ (bing search engine) [default]")
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())

            .DoCheck(f => f.The_current_query_is(), "wap edit https://bing.com/ -> https://bing.com/ bing search engine [default]")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Edit https://bing.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Edit the url https://bing.com/ (bing search engine) [default]")

            .DoAction(f => f.Write_query("wap edit https://bing.com/ -> https://bing.com/ bing microsoft search engine [pro]"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Edit https://bing.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Edit the url https://bing.com/ (bing search engine) [default] -> https://bing.com/ (bing microsoft search engine) [pro]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .DoAction(f => f.Display_bar_launcher())

            .DoAction(f => f.Write_query("wap list"))
            .EndUsing()


            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine) [default]")
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing microsoft search engine) [pro]")
            .Check("Start https://stackoverflow.com/", "Start the url https://stackoverflow.com/ (questions answers) [default]")
            .Check("Start https://netflix.com/", "Start the url https://netflix.com/ (video) [default]")
            .EndUsing()

            .EndTest();
    }
}
