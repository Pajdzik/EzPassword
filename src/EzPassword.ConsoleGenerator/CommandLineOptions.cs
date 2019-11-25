namespace EzPassword.ConsoleGenerator
{
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
    }
}