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
            PasswordGenerator generator = Program.CreatePasswordGenerator();
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddSingleton<PasswordGenerator>(_ => generator);
                }).Build();

            host.Run();
        }

        private static PasswordGenerator CreatePasswordGenerator()
        {
            string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            Console.WriteLine(dllPath);

            string wordsDirectory = Path.Combine(dllPath, "content", "pl");
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