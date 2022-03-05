using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    public class Open_url : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()
            .IsRunnable()
            .Include<Prepare_common_context_with_multiple_profiles>()

            .Using<BarLauncher_bar_fixture>()

            .DoAction(f => f.Write_query("wap ope"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "open URL")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Open an url as a web app without saving it")
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())

            .DoCheck(f => f.The_current_query_is(), "wap open ")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "open URL")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Open an url as a web app without saving it")

            .DoAction(f => f.Write_query("wap open https://example.com/"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "open https://example.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Open the web app page [https://example.com/] with profile [default] without saving it")

            .DoAction(f => f.Write_query("wap open https://example.com/ test"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "open URL")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Open an url as a web app without saving it")

            .DoAction(f => f.Write_query("wap open https://example.com/ [pro"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "open URL")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Open an url as a web app without saving it")

            .DoAction(f => f.Write_query("wap open https://example.com/ [pro]"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "open https://example.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Open the web app page [https://example.com/] with profile [pro] without saving it")

            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(r=>r.Command, r=>r.Arguments)
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(r => r.Command, r => r.Arguments)
            .Check("chrome.exe", "--app=\"https://example.com/\" --profile-directory=\"Pro\"")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap open https://example.com/"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "open https://example.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Open the web app page [https://example.com/] with profile [default] without saving it")
            .DoAction(f => f.Select_line(1))
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(r => r.Command, r => r.Arguments)
            .Check("chrome.exe", "--app=\"https://example.com/\" --profile-directory=\"Pro\"")
            .Check("chrome.exe", "--app=\"https://example.com/\" --profile-directory=\"Default\"")
            .EndUsing()

            .EndTest();
    }
}