using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    public class Remove_url : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()
            .IsRunnable()
            .Include<Prepare_common_context>()

            .Using<BarLauncher_bar_fixture>()

            .DoAction(f => f.Write_query("wap rem"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "remove [URL|PATTERN]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Remove an existing url")
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())

            .DoCheck(f => f.The_current_query_is(), "wap remove ")
            .DoCheck(f => f.The_number_of_results_is(), "4")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("remove https://google.com/", "Prepare to remove https://google.com/")
            .Check("remove https://bing.com/", "Prepare to remove https://bing.com/")
            .Check("remove https://stackoverflow.com/", "Prepare to remove https://stackoverflow.com/")
            .Check("remove https://netflix.com/", "Prepare to remove https://netflix.com/")
            .EndUsing()

            .Comment("Instead of writting an url, we're going to write a search term")

            .Using<BarLauncher_bar_fixture>()

            .DoAction(f => f.Append__on_query("bing"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "remove https://bing.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Prepare to remove https://bing.com/")

            .Comment("The subtitle indicate 'Prepare to remove', which mean it won't remove yet the url.")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap remove https://bing.com/")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "remove https://bing.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Remove the url https://bing.com/")

            .Comment("The query has been completed with the correct url. Now selecting the result will really remove the url. It serve as a confirmation.")
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap list"))

            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine) [default]")
            .Check("Start https://stackoverflow.com/", "Start the url https://stackoverflow.com/ (questions answers) [default]")
            .Check("Start https://netflix.com/", "Start the url https://netflix.com/ (video) [default]")
            .EndUsing()

            .EndTest();
    }
}