namespace EzPassword.ConsoleGenerator
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using CommandLine;
    using EzPassword.Core;
    using EzPassword.Core.Config;
    using EzPassword.Transformation;

    public class Program
    {
        public async static Task Main(string[] args)
        {
            ParserResult<CommandLineOptions> parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            await parserResult.MapResult(
                options =>
                {
                    var transformer = TransformerFactory.CreateFromKeywords(options.Transformations);

                    if (string.IsNullOrEmpty(options.WordsDirectory))
                    {
                        string? dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        options.WordsDirectory = Path.Combine(dllPath ?? "", "content");
                    }

                    var generator = PasswordGeneratorFactory.Create(
                        Path.Join(options.WordsDirectory, WordDirectoryConfig.AdjectiveDirectoryName),
                        Path.Join(options.WordsDirectory, WordDirectoryConfig.NounDirectoryName),
                        WordDirectoryConfig.AdjectiveFileNameRegex,
                        WordDirectoryConfig.NounFileNameRegex
                    );

                    int numberOfGeneratedPasswords = options.PasswordCount;
                    while (numberOfGeneratedPasswords-- > 0)
                    { 
                        var password = generator.Generate(options.PasswordLength);
                        var transformedPassword = transformer.Transform(password);
                        Console.WriteLine(transformedPassword + " (Original: \"" + password + "\")");
                    }

                    return Task.CompletedTask;
                },
                (_) => Task.CompletedTask)
                .ConfigureAwait(true);
        }
    }
}
