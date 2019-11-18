namespace EzPassword.WordDownloader
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using CommandLine;
    using Core.Config;
    using EzPassword.Core.Wiki;
    using Newtonsoft.Json;
    using NLog;
    using WikiClientLibrary.Client;
    using WikiClientLibrary.Generators;
    using WikiClientLibrary.Pages;
    using WikiClientLibrary.Sites;

    internal class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static async Task Main(string[] args)
        {
            ParserResult<CommandLineOptions> parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            await parserResult.MapResult(async options =>
            {
                IDictionary<string, Language> wikiConfig = ReadWikiConfig();
                Language language = wikiConfig[options.LanguageSymbol];
                await RunTasks(language, options.OutDirectory).ConfigureAwait(true);
            }, (_) => Task.CompletedTask).ConfigureAwait(true);
        }

        private static async Task RunTasks(Language language, string outDirectory)
        {
            using var client = new WikiClient
            {
                ClientUserAgent = "WCLQuickStart/1.0"
            };

            var site = new WikiSite(client, "https://pl.wiktionary.org/w/api.php");
            await site.Initialization.ConfigureAwait(true);

            var page = new WikiPage(site, "Kategoria:Język polski - przymiotniki");
            var generator = new CategoryMembersGenerator(page);
            var observable = generator.EnumPagesAsync().Where((page, b) => !page.IsSpecialPage).ToObservable();

            var observer = new TextFileWordPersister(outDirectory, WordDirectoryConfig.AdjectiveFileNameTemplate);
            observable.Subscribe(observer);

            await observable;
        }

        private static IDictionary<string, Language> ReadWikiConfig()
        {
            const string WikiConfigFileName = "WikiConfig.json";
            var wikiConfig = File.ReadAllText(WikiConfigFileName);
            var languages = JsonConvert.DeserializeObject<List<Language>>(wikiConfig);
            return languages.ToDictionary(lang => lang.Symbol, lang => lang);
        }
    }
}