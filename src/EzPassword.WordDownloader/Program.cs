namespace EzPassword.WordDownloader
{
    using System.Threading.Tasks;
    using WikiClientLibrary;
    using WikiClientLibrary.Client;
    using WikiClientLibrary.Generators;

    internal class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            var wikiClient = new WikiClient();
            var site = await Site.CreateAsync(wikiClient, "https://pl.wiktionary.org/w/api.php");
            var categoryMembersGenerator = new CategoryMembersGenerator(site, "Kategoria:Język polski - przymiotniki")
            {
                MemberTypes = CategoryMemberTypes. Page
            };

            var wikiDownloader = new WikiDownloader(categoryMembersGenerator);
            wikiDownloader.DownloadWords("C:\\Temp\\dicts", "adjectives_{0:D2}.txt");
        }
    }
}