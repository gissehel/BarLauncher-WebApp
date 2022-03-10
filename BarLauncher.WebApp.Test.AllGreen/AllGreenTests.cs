using AllGreen.Lib;
using AllGreen.Lib.Core.Engine.Service;
using AllGreen.Lib.Engine.Service;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using BarLauncher.EasyHelper;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen
{
    public class AllGreenTests
    {
        private static ITestRunnerService _testRunnerService = new TestRunnerService();

        private static TestFinder<WebAppContext> _testFinder = new TestFinder<WebAppContext>
        {
            Assembly = Assembly.GetExecutingAssembly(),
        };

        private static IEnumerable<string> GetTestScriptNames() => _testFinder.GetNames();

        [TestCaseSource(nameof(GetTestScriptNames))]
        public void Run(string name)
        {
            var testScript = _testFinder.GetTestScript(name);
            if (testScript != null)
            {
                var result = _testRunnerService.RunTest(testScript);

                Assert.IsNotNull(result, "The test returned a null result. Is the test runnable ?");
                Assert.IsTrue(result.Success, result.PipedName);
            }
            else
            {
                Assert.Fail("Don't know [{0}] as a test name !".FormatWith(name));
            }
        }
    }
}