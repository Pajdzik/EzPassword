﻿namespace EzPassword.WordDownloader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Threading.Tasks;
    using CommandLine;
    using EzPassword.Core.Config;
    using EzPassword.Core.Wiki;
    using Newtonsoft.Json;
    using WikiClientLibrary.Client;
    using WikiClientLibrary.Sites;

    internal class Program
    {
        // private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static async Task Main(string[] args)
        {
            ParserResult<CommandLineOptions> parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            await parserResult.MapResult(
                async options =>
                {
                    IDictionary<string, WikiLanguage> wikiConfig = ReadWikiConfig();
                    WikiLanguage language = wikiConfig[options.LanguageSymbol!];
                    string outDirectory = Path.Join(options.OutDirectory, language.Symbol);
                    await RunTasks(language, outDirectory).ConfigureAwait(true);
                },
                (_) => Task.CompletedTask)
                .ConfigureAwait(true);
        }

        private static async Task RunTasks(WikiLanguage language, string outDirectory)
        {
            using var client = new WikiClient
            {
                ClientUserAgent = Uri.EscapeDataString("github.com/Pajdziu/EzPassword"),
            };

            var site = new WikiSite(client, language.WikiApi.ToString());
            await site.Initialization.ConfigureAwait(true);

            var (adjectiveDownloader, nounDownloader) = WikiWordDownloaderFactory.CreateLanguageDownloaders(site, language);

            var adjectivePersister = new TextFileWordPersister(
                Path.Join(outDirectory, WordDirectoryConfig.AdjectiveDirectoryName),
                WordDirectoryConfig.AdjectiveFileNameTemplate);
            adjectiveDownloader.Subscribe(adjectivePersister);

            var nounPersister = new TextFileWordPersister(
                Path.Join(outDirectory, WordDirectoryConfig.NounDirectoryName),
                WordDirectoryConfig.NounFileNameTemplate);
            nounDownloader.Subscribe(nounPersister);

            await Task.WhenAll(adjectiveDownloader.ToTask(), nounDownloader.ToTask())
                .ConfigureAwait(true);
        }

        private static IDictionary<string, WikiLanguage> ReadWikiConfig()
        {
            const string WikiConfigFileName = "WikiConfig.json";
            var wikiConfig = File.ReadAllText(WikiConfigFileName);
            var languages = JsonConvert.DeserializeObject<List<WikiLanguage>>(wikiConfig);
            return languages.ToDictionary(lang => lang.Symbol, lang => lang);
        }
    }
}