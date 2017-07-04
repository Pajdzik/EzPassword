namespace EzPassword.WordDownloader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using CommandLine;
    using Core.Config;
    using Core.Wiki;
    using Newtonsoft.Json;
    using NLog;

    internal class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            var option = new CommandLineOptions();
            var isValid = Parser.Default.ParseArgumentsStrict(args, option);
            var wikiConfig = ReadWikiConfig();

            var language = wikiConfig[option.LanguageSymbol];

            var tasks = RunTasks(language, option.OutDirectory);

            Task.WhenAll(tasks).Wait();

            Console.WriteLine("Completed!");
        }

        private static IEnumerable<Task> RunTasks(Language language, string outDirectory)
        {
            Task nounTask = Task.Run(() =>
            {
                IObservable<string> nounGenerator = new WikiWordGenerator(language.NounCategory);
                var nounSubscriber = new TextFileWordPersister(
                    $@"{outDirectory}\{language.Symbol}\nouns",
                    "nouns_{0:D2}.txt");
                Logger.Info("Saving nouns starting with letters: ");
                nounGenerator.Subscribe(nounSubscriber);
                nounGenerator.Wait();
            });
            yield return nounTask;

            Task adjectiveTask = Task.Run(() =>
            {
                IObservable<string> adjectiveGenerator = new WikiWordGenerator(language.AdjectiveCategory);
                var adjectiveSubscriber = new TextFileWordPersister(
                    $@"{outDirectory}\{language.Symbol}\adjectives",
                    "adjectives_{0:D2}.txt");
                Logger.Info("Saving adjectives starting with letters: ");
                adjectiveGenerator.Subscribe(adjectiveSubscriber);
                adjectiveGenerator.Wait();
            });
            yield return adjectiveTask;
        }

        private static IDictionary<string, Language> ReadWikiConfig()
        {
            const string wikiConfigFileName = "WikiConfig.json";
            var wikiConfig = File.ReadAllText(wikiConfigFileName);
            var languages = JsonConvert.DeserializeObject<List<Language>>(wikiConfig);
            return languages.ToDictionary(lang => lang.Symbol, lang => lang);
        }
    }
}