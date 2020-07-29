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
            Parser parser = new Parser(config => { config.CaseInsensitiveEnumValues = true; });
            ParserResult<CommandLineOptions> parserResult = parser.ParseArguments<CommandLineOptions>(args);
            await parserResult.MapResult(
                options =>
                {
                    Generate(options);
                    return Task.CompletedTask;
                },
                (_) => Task.CompletedTask)
                .ConfigureAwait(true);
        }

        private static Task Generate(CommandLineOptions options)
        {
            try
            {
                var transformer = TransformerFactory.CreateFromKeywords(options.Transformations);

                if (string.IsNullOrEmpty(options.WordsDirectory))
                {
                    string? dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    options.WordsDirectory = Path.Combine(dllPath ?? "", "content", Language.Get(options.Language));
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
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error while generating password:\n\n{0}", ex);
            }

            return Task.CompletedTask;
        }
    }
}
