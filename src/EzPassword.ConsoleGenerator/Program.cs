namespace EzPassword.ConsoleGenerator
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CommandLine;
    using EzPassword.Core;
    using EzPassword.Core.Config;
    using EzPassword.Transformation;
    using Kpax.Abstraction.System.IO;

    public class Program
    {
        public async static Task Main(string[] args)
        {
            ParserResult<CommandLineOptions> parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            await parserResult.MapResult(
                options =>
                {
                    var adjectiveGenerator = new TextFileWordGenerator(
                        new DirectoryFacade(),
                        new FileFacade(),
                        Path.Join(options.WordsDirectory, WordDirectoryConfig.AdjectiveDirectoryName),
                        WordDirectoryConfig.AdjectiveFileNameRegex);

                    var nounGenerator = new TextFileWordGenerator(
                        new DirectoryFacade(),
                        new FileFacade(),
                        Path.Join(options.WordsDirectory, WordDirectoryConfig.NounDirectoryName),
                        WordDirectoryConfig.NounFileNameRegex);

                    var transformer = TransformerFactory.CreateFromKeywords(options.Transformations);

                    var generator = new PasswordGenerator(adjectiveGenerator, nounGenerator);

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
