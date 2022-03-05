using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    public class Prepare_common_context : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()

            .Using<Application_information_fixture>()
            .DoAction(f => f.Application_name_is("BarLauncher-WebApp"))
            .DoAction(f => f.Application_verison_is("0.0"))
            .DoAction(f => f.Application_url_is("https://github.com/gissehel/BarLauncher-WebApp"))
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Start_the_bar())

            .DoAction(f => f.Display_bar_launcher())
            .DoCheck(f => f.The_current_query_is(), "")
            .DoAction(f => f.Write_query("wap add https://google.com/ google search engine"))
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap add https://bing.com/ bing search engine"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap add https://stackoverflow.com/ questions answers"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("wap add https://netflix.com/ video"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query(""))
            .EndUsing()

            .EndTest();
    }
}