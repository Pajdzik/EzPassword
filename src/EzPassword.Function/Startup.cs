
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(EzPassword.Function.Startup))]
namespace EzPassword.Function
{
    using System.IO;
    using System.Reflection;
    using EzPassword.Core;
    using EzPassword.Core.Config;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            string wordsDirectory = Path.Combine(dllPath, "..", "content", "pl");

            var passwordGenerator = PasswordGeneratorFactory.Create(
                    Path.Join(wordsDirectory, "adjectives"),
                    Path.Join(wordsDirectory, "nouns"),
                    WordDirectoryConfig.AdjectiveFileNameRegex,
                    WordDirectoryConfig.NounFileNameRegex
                );

            builder.Services.AddSingleton<PasswordGenerator>((_) => passwordGenerator);
        }
    }
}
