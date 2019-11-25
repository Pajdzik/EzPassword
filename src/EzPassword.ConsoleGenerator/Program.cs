﻿namespace EzPassword.ConsoleGenerator
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CommandLine;
    using EzPassword.Core;
    using EzPassword.Core.Config;
    using Kpax.Abstraction.System.IO;

    public class Program
    {
        public async static Task Main(string[] args)
        {
            ParserResult<CommandLineOptions> parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            await parserResult.MapResult(
                async options =>
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

                    var generator = new PasswordGenerator(adjectiveGenerator, nounGenerator);

                    int numberOfGeneratedPasswords = options.PasswordCount;
                    while (numberOfGeneratedPasswords-- > 0) { 
                        var password = generator.Generate(options.PasswordLength);
                        Console.WriteLine(password);
                    }
                },
                (_) => Task.CompletedTask)
                .ConfigureAwait(true);
        }
    }
}