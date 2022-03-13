using System.Reflection;
using Xunit.Sdk;

namespace BarLauncher.WebApp.Test.Unit
{
    public class SetContextAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            Helper.TestName = methodUnderTest.Name;
        }

        public override void After(MethodInfo methodUnderTest)
        {
            Helper.TestName = null;
        }
    }
}
