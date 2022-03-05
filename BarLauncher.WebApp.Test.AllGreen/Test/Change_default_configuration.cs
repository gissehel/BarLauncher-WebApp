using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    public class Change_default_configuration : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Comment("We're going to reconfigure the plugin with a new web app executable for default profile")

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

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap config default ")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config default chrome.exe --app=\"{0}\" --profile-directory=\"Default\"")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change default webapp launcher to [chrome.exe] and argument to [--app=\"{0}\" --profile-directory=\"Default\"]")

            .Comment("Now the query is 'wap config default ', we expect that what look like the same result will make completion to the actual configuration.")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap config default chrome.exe --app=\"{0}\" --profile-directory=\"Default\"")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config default chrome.exe --app=\"{0}\" --profile-directory=\"Default\"")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change default webapp launcher to [chrome.exe] and argument to [--app=\"{0}\" --profile-directory=\"Default\"]")

            .Comment("We're now going to change only the launcher and not the argument pattern.")

            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoAction(f => f.Write_query("wap config default mywepapplauncher.exe"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config default mywepapplauncher.exe [APP_ARGUMENT_PATTERN]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change default webapp launcher to [mywepapplauncher.exe] and argument to [--app=\"{0}\" --profile-directory=\"Default\"]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())

            .Comment("We expect now the configuration to be changed. We're going to verify that.")

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap video"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Start https://netflix.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Start the url https://netflix.com/ (video) [default]")

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
            .Check("mywepapplauncher.exe", "--app=\"https://netflix.com/\" --profile-directory=\"Default\"")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap config default mywepapplauncher.exe "))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config default mywepapplauncher.exe [APP_ARGUMENT_PATTERN]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change default webapp launcher to [mywepapplauncher.exe] and argument to [--app=\"{0}\" --profile-directory=\"Default\"]")

            .DoAction(f => f.Append__on_query("x"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config default mywepapplauncher.exe x")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "You should consider having [{0}] inside arguments. Now it contains only [x]")

            .DoAction(f => f.Write_query("wap config default mywepapplauncher.exe {0}"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config default mywepapplauncher.exe {0}")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change default webapp launcher to [mywepapplauncher.exe] and argument to [{0}]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())

            .Comment("We expect now the configuration to be changed. We're going to verify that.")

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap video"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Start https://netflix.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Start the url https://netflix.com/ (video) [default]")

            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check("mywepapplauncher.exe", "--app=\"https://netflix.com/\" --profile-directory=\"Default\"")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check("mywepapplauncher.exe", "--app=\"https://netflix.com/\" --profile-directory=\"Default\"")
            .Check("mywepapplauncher.exe", "https://netflix.com/")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())

            .DoAction(f => f.Write_query("wap config default mywepapplauncher.exe --app=\"{0}\" --profile-directory=\"Default\" --maximized"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config default mywepapplauncher.exe --app=\"{0}\" --profile-directory=\"Default\" --maximized")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change default webapp launcher to [mywepapplauncher.exe] and argument to [--app=\"{0}\" --profile-directory=\"Default\" --maximized]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())

            .Comment("We expect now the configuration to be changed. We're going to verify that.")

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap video"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Start https://netflix.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Start the url https://netflix.com/ (video) [default]")
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())

            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check("mywepapplauncher.exe", "--app=\"https://netflix.com/\" --profile-directory=\"Default\"")
            .Check("mywepapplauncher.exe", "https://netflix.com/")
            .Check("mywepapplauncher.exe", "--app=\"https://netflix.com/\" --profile-directory=\"Default\" --maximized")
            .EndUsing()

            .EndTest();
    }
}