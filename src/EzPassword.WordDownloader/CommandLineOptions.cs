namespace EzPassword.WordDownloader
{
    using System.Diagnostics.CodeAnalysis;
    using CommandLine;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Used by reflection")]
    internal class CommandLineOptions
    {
        [Option('l', "lang", Required = true, HelpText = "Symbol of the language to download")]
        public string? LanguageSymbol { get; set; }

        [Option('o', "out", Required = true, HelpText = "Out dictionary location")]
        public string? OutDirectory { get; set; }
    }
}