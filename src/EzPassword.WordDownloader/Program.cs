namespace EzPassword.WordDownloader
{
    using System.Threading.Tasks;
    using Core.Config;
    using WikiClientLibrary;
    using WikiClientLibrary.Client;
    using WikiClientLibrary.Generators;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var wikiDownloader = new WikiDownloader(Languages.Polish.AdjectiveCategory);
            wikiDownloader.DownloadWords(@"C:\Temp\dicts\pl", "adjectives_{0:D2}.txt");
        }
    }
}