using AllGreen.Lib;
using System.Collections.Generic;
using BarLauncher.EasyHelper.Test.Mock.Service;
using BarLauncher.WebApp.Test.AllGreen.Helper;

namespace BarLauncher.WebApp.Test.AllGreen.Fixture
{
    public class Last_file_generated_fixture : FixtureBase<WebAppContext>
    {
        public class Result
        {
            public string Line { get; set; }
        }

        public override IEnumerable<object> OnQuery()
        {
            foreach (var line in LastFileGenerator.Lines)
            {
                yield return new Result
                {
                    Line = line
                };
            }
        }

        public string The_filename_is() => LastFileGenerator.Path;

        private FileGeneratorMock LastFileGenerator => Context.ApplicationStarter.FileGeneratorService.LastFileGenerator;
    }
}