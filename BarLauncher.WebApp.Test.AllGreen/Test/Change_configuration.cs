using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    public class Change_configuration : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Comment("We're going to reconfigure the plugin to manage several profiles")

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("wap con"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("config [PROFILE] [APP_PATH] [APP_ARGUMENT_PATTERN]", "Configure a new webapp launcher for a profile")
            .EndUsing()

            .Comment("When we select the only result for query 'wap con', we expect the completion to 'wap config '.")

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap config ")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config default [APP_PATH] [APP_ARGUMENT_PATTERN]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Configure the default launcher - Select this item to complete the current config")

            .DoAction(f => f.Write_query("wap config pro"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config pro [APP_PATH] [APP_ARGUMENT_PATTERN]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Create a pro webapp launcher")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap config pro chrome.exe --app=\"{0}\" --profile-directory=\"Default\"")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config pro chrome.exe --app=\"{0}\" --profile-directory=\"Default\"")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change pro webapp launcher to [chrome.exe] and argument to [--app=\"{0}\" --profile-directory=\"Default\"]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .DoAction(f => f.Display_bar_launcher())

            .DoAction(f => f.Write_query("wap config pro chrome.exe --app=\"{0}\" --profile-directory=\"Pro\""))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config pro chrome.exe --app=\"{0}\" --profile-directory=\"Pro\"")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change pro webapp launcher to [chrome.exe] and argument to [--app=\"{0}\" --profile-directory=\"Pro\"]")

            .EndUsing()

            .EndTest();
    }
}
