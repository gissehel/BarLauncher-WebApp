using AllGreen.Lib;
using BarLauncher.WebApp.Test.AllGreen.Fixture;
using BarLauncher.WebApp.Test.AllGreen.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarLauncher.WebApp.Test.AllGreen.Test
{
    public class Add_url_with_multiple_profiles : TestBase<WebAppContext>
    {
        public override void DoTest() =>
            StartTest()
            .IsRunnable()

            .Include<Prepare_common_context_with_multiple_profiles>()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("wap add https://example.com/ keyword "))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("add https://example.com/ keyword [default]", "Add the url https://example.com/ with keywords (keyword) and using profile [default]")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Append__on_query("["))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("add https://example.com/ keyword [default]", "Add the url https://example.com/ with keywords (keyword) and using profile [default]")
            .Check("add https://example.com/ keyword [pro]", "Add the url https://example.com/ with keywords (keyword) and using profile [pro]")
            .EndUsing()

            .EndTest();
    }
}
