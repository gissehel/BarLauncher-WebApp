using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    public class Import_urls_from_file : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .UsingSetup<Generate_file_fixture>()
            .With(f => f.Line)
            .Enter("# launcher: chrome.exe")
            .Enter("# argumentsPattern: --app=\"{0}\" --profile-directory=\"Default\"")
            .Enter("# launcher[pro]: msedge.exe")
            .Enter("# argumentsPattern[pro]: --app=\"{0}\" --profile-directory=\"Pro\"")
            .Enter("https://github.com/ (dev opensource repository)")
            .Enter("https://microsoft.com/ (corporate windows) [pro]")
            .EndUsing()

            .Using<Generate_file_fixture>()
            .DoAction(f => f.Save_last_file_to(@"C:\path\on\filesystem\filename.wap.txt"))
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()

            .DoAction(f => f.Write_query("wap imp"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "import FILENAME")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Import urls from FILENAME")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())

            .DoCheck(f => f.The_current_query_is(), "wap import ")
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "import FILENAME")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Import urls from FILENAME")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap import ")

            .DoAction(f => f.Write_query("wap import filename.wap.txt"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "import filename.wap.txt")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "[filename.wap.txt] does not exists")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "wap import filename.wap.txt")

            .DoAction(f => f.Write_query(@"wap import C:\path\on\filesystem\filename.wap"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), @"import C:\path\on\filesystem\filename.wap")
            .DoCheck(f => f.The_subtitle_of_result__is(1), @"[C:\path\on\filesystem\filename.wap] does not exists")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), @"wap import C:\path\on\filesystem\filename.wap")

            .DoAction(f => f.Write_query(@"wap import C:\path\on\filesystem\filename.wap.txt"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), @"import C:\path\on\filesystem\filename.wap.txt")
            .DoCheck(f => f.The_subtitle_of_result__is(1), @"Import urls from [C:\path\on\filesystem\filename.wap.txt]")

            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .DoAction(f => f.Display_bar_launcher())

            .DoAction(f => f.Write_query(@"wap git"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Start https://github.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Start the url https://github.com/ (dev opensource repository) [default]")

            .DoAction(f => f.Write_query(@"wap windo"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "Start https://microsoft.com/")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Start the url https://microsoft.com/ (corporate windows) [pro]")

            .DoAction(f => f.Write_query(@"wap config"))
            .DoCheck(f => f.The_number_of_results_is(), "2")
            .DoCheck(f => f.The_title_of_result__is(1), "config default [APP_PATH] [APP_ARGUMENT_PATTERN]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Configure the default launcher - Select this item to complete the current config")
            .DoCheck(f => f.The_title_of_result__is(2), "config pro [APP_PATH] [APP_ARGUMENT_PATTERN]")
            .DoCheck(f => f.The_subtitle_of_result__is(2), "Configure the pro launcher - Select this item to complete the current config")

            .DoAction(f => f.Write_query(@"wap config pro"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "config pro msedge.exe --app=\"{0}\" --profile-directory=\"Pro\"")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Change pro webapp launcher to [msedge.exe] and argument to [--app=\"{0}\" --profile-directory=\"Pro\"]")

            .EndUsing()

            .EndTest();
    }
}