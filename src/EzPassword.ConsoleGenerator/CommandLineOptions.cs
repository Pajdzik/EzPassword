namespace EzPassword.ConsoleGenerator
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using CommandLine;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Used by reflection")]
    internal class CommandLineOptions
    {
        [Option('c', "count", Required = true, HelpText = "How many passwords to generate [default: 5]")]
        public int PasswordCount { get; set; } = 5;

        [Option('l', "length", Required = true, HelpText = "Desired length of the generated password [default: 14]")]
        public int PasswordLength { get; set; } = 14;

        [Option('w', "words", Required = true, HelpText = "Path to the directory with words to generate passwords from")]
        public string? WordsDirectory { get; set; }

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