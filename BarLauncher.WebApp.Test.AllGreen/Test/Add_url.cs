using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    internal class Add_url : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()
            .IsRunnable()
            .Include<Prepare_common_context_with_multiple_profiles>()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("wap ad"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "add URL [KEYWORD] [KEYWORD] [...]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Add a new url (or update an existing) with associated keywords")
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())

            .DoCheck(f => f.The_current_query_is(), "wap add ")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "add URL [KEYWORD] [KEYWORD] [...]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Add a new url (or update an existing) with associated keywords")

            .DoAction(f => f.Write_query("wap add https://examp"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "add https://examp [default]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Add the url https://examp with no keywords and using profile [default]")

            .DoAction(f => f.Write_query("wap add https://example.com/"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "add https://example.com/ [default]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Add the url https://example.com/ with no keywords and using profile [default]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())

            .DoAction(f=>f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap list"))

            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine) [default]")
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing search engine) [pro]")
            .Check("Start https://stackoverflow.com/", "Start the url https://stackoverflow.com/ (questions answers) [default]")
            .Check("Start https://netflix.com/", "Start the url https://netflix.com/ (video) [default]")
            .Check("Start https://example.com/", "Start the url https://example.com/ () [default]")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()

            .DoAction(f => f.Write_query("wap add https://pro.example.com/ [pro]"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "add https://pro.example.com/ [pro]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Add the url https://pro.example.com/ with no keywords and using profile [pro]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())

            .DoAction(f => f.Display_bar_launcher())

            .DoAction(f => f.Write_query("wap add https://keys.example.com/ mank shon"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "add https://keys.example.com/ mank shon [default]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Add the url https://keys.example.com/ with keywords (mank shon) and using profile [default]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())

            .DoAction(f => f.Display_bar_launcher())

            .DoAction(f => f.Write_query("wap add https://prokeys.example.com/ blot rebt [pro]"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "add https://prokeys.example.com/ blot rebt [pro]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Add the url https://prokeys.example.com/ with keywords (blot rebt) and using profile [pro]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap list"))

            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("Start https://google.com/", "Start the url https://google.com/ (google search engine) [default]")
            .Check("Start https://bing.com/", "Start the url https://bing.com/ (bing search engine) [pro]")
            .Check("Start https://stackoverflow.com/", "Start the url https://stackoverflow.com/ (questions answers) [default]")
            .Check("Start https://netflix.com/", "Start the url https://netflix.com/ (video) [default]")
            .Check("Start https://example.com/", "Start the url https://example.com/ () [default]")
            .Check("Start https://pro.example.com/", "Start the url https://pro.example.com/ () [pro]")
            .Check("Start https://keys.example.com/", "Start the url https://keys.example.com/ (mank shon) [default]")
            .Check("Start https://prokeys.example.com/", "Start the url https://prokeys.example.com/ (blot rebt) [pro]")
            .EndUsing()

            .EndTest();
    }
}
