namespace EzPassword.Function
{
    using System;
    using System.IO;
    using System.Reflection;
    using EzPassword.Core;
    using EzPassword.Core.Config;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main()
        {
            MultiLangPasswordGenerator generator = CreateMultiLangPasswordGenerator();
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddSingleton<MultiLangPasswordGenerator>(_ => generator);
                }).Build();

            host.Run();
        }

        private static MultiLangPasswordGenerator CreateMultiLangPasswordGenerator()
        {
            PasswordGenerator englishGenerator = CreatePasswordGenerator(Language.Name.English);
            PasswordGenerator polishGenerator = CreatePasswordGenerator(Language.Name.Polish);
            
            return new MultiLangPasswordGenerator(new [] { (Language.Name.English, englishGenerator), (Language.Name.Polish, polishGenerator) });
        }

        private static PasswordGenerator CreatePasswordGenerator(Language.Name lang)
        {
            string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            Console.WriteLine(dllPath);

            string wordsDirectory = Path.Combine(dllPath, "content", Language.Get(lang));
            Console.WriteLine(wordsDirectory);

            return PasswordGeneratorFactory.Create(
                    Path.Join(wordsDirectory, "adjectives"),
                    Path.Join(wordsDirectory, "nouns"),
                    WordDirectoryConfig.AdjectiveFileNameRegex,
                    WordDirectoryConfig.NounFileNameRegex
                );
        }
    }
}