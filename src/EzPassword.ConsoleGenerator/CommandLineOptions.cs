namespace EzPassword.ConsoleGenerator
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using CommandLine;
    using EzPassword.Core;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Used by reflection")]
    internal class CommandLineOptions : IPasswordParameters
    {
        [Option("lang", HelpText = "Language of the used words", Default = Core.Language.Name.English)]
        public Language.Name Language { get; set; }

        [Option('c', "count", HelpText = "How many passwords to generate", Default = 5)]
        public int PasswordCount { get; set; }

        [Option('l', "length", HelpText = "Desired length of the generated password", Default = 20)]
        public int PasswordLength { get; set; }

        [Option('w', "words", HelpText = "Path to the directory with words to generate passwords from", Default = "")]
        public string WordsDirectory { get; set; } = "";

        [
            Option('t', "transformations", Required = false,
            HelpText = "List of password transformations.\n"
                     + "  Available:\n"
                     + "    * space - adds space between words\n"
                     + "    * special - adds special characters between words\n"
                     + "    * camel - converts words to camel case\n"
                     + "    * upper - converts words to upper case\n"
                     + "    * lower - converts words to lower case\n"
                     + "    * l33t - converts words to l33t speak"
        )]
        public IEnumerable<string> Transformations { get; set; } = new List<string>(0);
    }
}