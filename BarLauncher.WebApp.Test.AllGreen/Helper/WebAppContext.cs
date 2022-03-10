using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.ScriptResult;
using AllGreen.Lib.Engine.Service;
using System.IO;
using System.Text;

namespace BarLauncher.WebApp.Test.AllGreen.Helper
{
    public class WebAppContext : IContext<WebAppContext>
    {
        public ITestScript<WebAppContext> TestScript { get; set; }
        public ITestScriptResult<WebAppContext> TestScriptResult { get; set; }

        public ApplicationStarter ApplicationStarter { get; set; }

        public void OnTestStart()
        {
            ApplicationStarter = new ApplicationStarter();
            ApplicationStarter.Init(TestScript.Name);
        }

        public void OnTestStop()
        {
            var testScriptResult = TestScriptResult as TestScriptResult<WebAppContext>;
            (new JsonOutputService()).Output(testScriptResult, ApplicationStarter.TestPath, testScriptResult.TestScript.Name);
            (new TextOutputService()).Output(testScriptResult, ApplicationStarter.TestPath, testScriptResult.TestScript.Name);
        }
    }
}