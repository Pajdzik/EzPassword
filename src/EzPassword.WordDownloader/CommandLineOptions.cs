namespace EzPassword.WordDownloader
{
    using CommandLine;

    internal class CommandLineOptions
    {
        [Option('l', "lang", Required = true, HelpText = "Symbol of the language to download")]
        public string LanguageSymbol { get; set; }

        [Option('o', "out", Required = true, HelpText = "Out dictionary location")]
        public string OutDirectory { get; set; }
    }
}