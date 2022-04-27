using System.Collections.Generic;
using System.Linq;
using BarLauncher.EasyHelper;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.WebApp.Lib.Core.Service;

namespace BarLauncher.WebApp.Lib.Service
{
    public class WebAppResultFinder : BarLauncherResultFinder
    {
        private IWebAppService WebAppService { get; set; }
        private IHelperService HelperService { get; set; }

        private IApplicationInformationService ApplicationInformationService { get; set; }

        private ISystemService SystemWebAppService { get; set; }

        public WebAppResultFinder(IBarLauncherContextService barLauncherContextService, IWebAppService webAppService, IHelperService helperService, IApplicationInformationService applicationInformationService, ISystemService systemWebAppService) : base(barLauncherContextService)
        {
            WebAppService = webAppService;
            HelperService = helperService;
            ApplicationInformationService = applicationInformationService;
            SystemWebAppService = systemWebAppService;
        }

        public override void Init()
        {
            WebAppService.Init();

            AddCommand("list", "list [PATTERN] [PATTERN] [...]", "List all url matching patterns", GetListResults);
            AddCommand("config", "config [PROFILE] [APP_PATH] [APP_ARGUMENT_PATTERN]", "Configure a new webapp launcher for a profile", GetConfigResults);
            AddCommand("add", "add URL [KEYWORD] [KEYWORD] [...]", "Add a new url (or update an existing) with associated keywords", GetAddResults);
            AddCommand("remove", "remove [URL|PATTERN]", "Remove an existing url", GetRemoveResults);
            AddCommand("edit", "edit [URL|PATTERN] [ -> URL [KEYWORD] [KEYWORD] [...] [\\[PROFILE\\]]", "Edit an existing url", GetEditResults);
            AddCommand("open", "open URL", "Open an url as a web app without saving it", GetOpenResults);
            AddCommand("export", "export", "Export urls to a file", ExportCommandAction);
            AddCommand("import", "import FILENAME", "Import urls from FILENAME", GetImportResults);
            AddCommand("help", "help", HelpSubtitle, HelpCommand);

            AddDefaultCommand(GetListResults);
        }

        private string HelpSubtitle => string.Format("{0} version {1} - (Go to {0} main web site)", ApplicationInformationService.ApplicationName, ApplicationInformationService.Version);

        private void HelpCommand() => SystemWebAppService.OpenUrl(ApplicationInformationService.HomepageUrl);

        private IEnumerable<BarLauncherResult> GetEditResults(BarLauncherQuery query, int position)
        {
            if (query.SearchTerms.Contains("->"))
            {
                var url = query.GetTermOrEmpty(1);
                var oper = query.GetTermOrEmpty(2);
                if (oper == "->")
                {
                    var newUrl = query.GetTermOrEmpty(3);
                    var newKeywords = query.GetAllSearchTermsStarting(4);
                    string newProfile = null;
                    if (!HelperService.ExtractProfile(newKeywords,ref newKeywords, ref newProfile))
                    {
                        newProfile = "default";
                    }
                    var webAppItem = WebAppService.GetUrlInfo(url);
                    var keywords = webAppItem.Keywords;
                    var profile = webAppItem.Profile;

                    if ((keywords == newKeywords) && (url == newUrl) && (profile == newProfile))
                    {
                        return new List<BarLauncherResult> {
                        GetCompletionResultFinal(
                            string.Format("Edit {0}",url),
                            string.Format("Edit the url {0} ({1}) [{2}]", url, keywords, profile),
                            ()=>string.Format("edit {0} -> {0} {1} [{2}]", url, keywords, profile)
                        )
                    };
                    }
                    else
                    {
                        return new List<BarLauncherResult> {
                        GetActionResult(
                            string.Format("Edit {0}",url),
                            string.Format("Edit the url {0} ({1}) [{2}] -> {3} ({4}) [{5}]", url, keywords, profile, newUrl, newKeywords, newProfile),
                            () =>
                            {
                                WebAppService.EditWebAppItem(url, newUrl, newKeywords, newProfile);
                            }
                        )
                    };
                    }
                }
                else
                {
                    return new List<BarLauncherResult> {
                        GetCompletionResult(
                            "edit [URL|PATTERN] [ -> URL [KEYWORD] [KEYWORD] [...]]",
                            "Edit an existing url",
                            ()=>"edit "
                        )
                    };
                }

                throw new System.NotImplementedException();
            }
            else
            {
                var terms = query.GetSearchTermsStarting(position);
                return WebAppService
                    .Search(terms)
                    .Select
                    (
                        item => GetCompletionResultFinal
                        (
                            string.Format("Edit {0}", item.Url),
                            string.Format("Edit the url {0} ({1}) [{2}]", item.Url, item.Keywords, item.Profile),
                            () => string.Format("edit {0} -> {0} {1} [{2}]", item.Url, item.Keywords, item.Profile)
                        )
                    );
            }
        }

        private IEnumerable<BarLauncherResult> GetListResults(BarLauncherQuery query, int position)
        {
            var terms = query.GetSearchTermsStarting(position);
            return WebAppService
                .Search(terms)
                .Select
                (
                    item => GetActionResult
                    (
                        string.Format("Start {0}", item.Url),
                        string.Format("Start the url {0} ({1}) [{2}]", item.Url, item.Keywords, item.Profile),
                        () =>
                        {
                            WebAppService.StartUrl(item.Url, item.Profile);
                        }
                    )
                );
        }

        private IEnumerable<BarLauncherResult> GetConfigResults(BarLauncherQuery query, int position)
        {
            if (query.SearchTerms.Length > position)
            {
                var profile = query.SearchTerms[position];
                var configuration = WebAppService.GetConfiguration(profile);

                if (query.SearchTerms.Length > position + 1)
                {
                    var launcher = query.SearchTerms[position + 1];
                    string arguments = query.GetAllSearchTermsStarting(position + 2);
                    string argumentsCommandLine = arguments ?? "[APP_ARGUMENT_PATTERN]";
                    string argumentsReal = arguments ?? configuration.WebAppArgumentPattern;

                    string title = string.Format("config {0} {1} {2}", profile, launcher, argumentsCommandLine);
                    string subTitle = string.Format("Change {0} webapp launcher to [{1}] and argument to [{2}]", profile, launcher, argumentsReal);
                    if (!argumentsReal.Contains("{0}"))
                    {
                        subTitle = string.Format("You should consider having [{0}] inside arguments. Now it contains only [{1}]", "{0}", argumentsReal);
                    }
                    yield return GetActionResult
                    (
                        title,
                        subTitle,
                        () =>
                        {
                            WebAppService.UpdateLauncher(launcher, argumentsReal, profile);
                        }
                    );
                } 
                else
                {
                    if (configuration == null)
                    {
                        yield return GetCompletionResultFinal
                            (
                                string.Format("config {0} [APP_PATH] [APP_ARGUMENT_PATTERN]", profile),
                                "Create a {0} webapp launcher".FormatWith(profile),
                                () => {
                                    configuration = WebAppService.GetOrCreateConfiguration(profile);
                                    return string.Format("config {0} {1} {2}", profile, configuration.WebAppLauncher, configuration.WebAppArgumentPattern);
                                }
                            );
                    }
                    else
                    {
                        yield return GetCompletionResultFinal
                            (
                                string.Format("config {0} {1} {2}", profile, configuration.WebAppLauncher, configuration.WebAppArgumentPattern),
                                "Change {0} webapp launcher to [{1}] and argument to [{2}]".FormatWith(profile, configuration.WebAppLauncher, configuration.WebAppArgumentPattern),
                                () => string.Format("config {0} {1} {2}", profile, configuration.WebAppLauncher, configuration.WebAppArgumentPattern)
                            );
                    }
                }
            }
            else
            {
                var emptyResult = GetEmptyCommandResult("config", CommandInfos);
                foreach (var profile in WebAppService.GetProfiles())
                {
                    var configuration = WebAppService.GetConfiguration(profile);
                    yield return GetCompletionResult
                        (
                            string.Format("config {0} [APP_PATH] [APP_ARGUMENT_PATTERN]", profile),
                            "Configure the {0} launcher - Select this item to complete the current config".FormatWith(profile),
                            () => string.Format("config {0}", profile)
                        );
                }
            }
        }

        private IEnumerable<BarLauncherResult> GetAddResults(BarLauncherQuery query, int position)
        {
            var url = query.GetTermOrEmpty(position);
            if (!string.IsNullOrEmpty(url))
            {
                var keywords = query.GetAllSearchTermsStarting(position + 1);
                string profile = "default";
                if (keywords == null)
                {
                    keywords = string.Empty;
                }
                else
                {
                    HelperService.ExtractProfile(keywords, ref keywords, ref profile);
                }
                keywords = keywords.Trim(' ');
                var profiles = WebAppService.GetProfiles();
                if (keywords == "")
                {
                    foreach (var existingProfile in profiles)
                    {
                        if (existingProfile.Contains(profile))
                        {
                            yield return GetActionResult
                            (
                                string.Format("add {0} [{1}]", url, existingProfile),
                                string.Format("Add the url {0} with no keywords and using profile [{1}]", url, existingProfile),
                                () =>
                                {
                                    WebAppService.AddWebAppItem(url, keywords, existingProfile);
                                }
                            );
                        }
                    }

                }
                else
                {
                    foreach (var existingProfile in profiles)
                    {
                        if (existingProfile.Contains(profile))
                        {
                            yield return GetActionResult
                            (
                                string.Format("add {0} {1} [{2}]", url, keywords, existingProfile),
                                string.Format("Add the url {0} with keywords ({1}) and using profile [{2}]", url, keywords, existingProfile),
                                () =>
                                {
                                    WebAppService.AddWebAppItem(url, keywords, existingProfile);
                                }
                            );
                        }
                    }
                }
            }
            else
            {
                yield return GetEmptyCommandResult("add", CommandInfos);
            }
        }

        private IEnumerable<BarLauncherResult> GetRemoveResults(BarLauncherQuery query, int position)
        {
            var webAppItems = WebAppService
                .Search(query.GetSearchTermsStarting(position));
            string urlTyped = null;
            if (query.SearchTerms.Length == position + 1)
            {
                urlTyped = query.SearchTerms[position];
            }
            var results = webAppItems
                .Select
                (
                    item =>
                        (urlTyped != null && item.Url == urlTyped)
                        ?
                        GetActionResult
                        (
                            string.Format("remove {0}", urlTyped),
                            string.Format("Remove the url {0}", urlTyped),
                            () => WebAppService.RemoveUrl(urlTyped)
                        )
                        :
                        GetCompletionResultFinal
                        (
                            string.Format("remove {0}", item.Url),
                            string.Format("Prepare to remove {0}", item.Url),
                            () => string.Format("remove {0}", item.Url)
                        )
                );
            if (results.Count() > 0)
            {
                foreach (var result in results)
                {
                    yield return result;
                }
            }
            else
            {
                yield return GetEmptyCommandResult("remove", CommandInfos);
            }
        }

        private IEnumerable<BarLauncherResult> GetOpenResults(BarLauncherQuery query, int position)
        {
            string profile = "default";
            bool isValid = false;

            if (query.SearchTerms.Length == position + 1)
            {
                isValid = true;
            }
            if ((query.SearchTerms.Length == position + 2) && query.SearchTerms[position + 1].StartsWith("[") && query.SearchTerms[position + 1].EndsWith("]"))
            {
                isValid = true;
                profile = query.SearchTerms[position + 1].TrimStart('[').TrimEnd(']');
            }

            if (isValid)
            {
                var url = query.SearchTerms[position];
                yield return GetActionResult
                (
                    "open {0}".FormatWith(url),
                    "Open the web app page [{0}] with profile [{1}] without saving it".FormatWith(url, profile),
                    () =>
                    {
                        WebAppService.StartUrl(url, profile);
                    }
                );
            }
            else
            {
                yield return GetEmptyCommandResult("open", CommandInfos);
            }
        }

        private void ExportCommandAction() => WebAppService.Export();

        private IEnumerable<BarLauncherResult> GetImportResults(BarLauncherQuery query, int position)
        {
            var filename = query.GetAllSearchTermsStarting(position);
            if (!string.IsNullOrEmpty(filename))
            {
                if (WebAppService.FileExists(filename))
                {
                    yield return GetActionResult
                    (
                        "import {0}".FormatWith(filename),
                        "Import urls from [{0}]".FormatWith(filename),
                        () => WebAppService.Import(filename)
                    );
                }
                else
                {
                    yield return GetCompletionResultFinal
                    (
                        "import {0}".FormatWith(filename),
                        "[{0}] does not exists".FormatWith(filename),
                        () => "import {0}".FormatWith(filename)
                    );
                }
            }
            else
            {
                yield return GetEmptyCommandResult("import", CommandInfos);
            }
        }

        public override void Dispose()
        {
            WebAppService.Dispose();
        }
    }
}