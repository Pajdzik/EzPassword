namespace EzPassword.Core.Wiki
{
    using System;
    using System.Linq;
    using EzPassword.Core.Config;
    using WikiClientLibrary.Generators;
    using WikiClientLibrary.Pages;
    using WikiClientLibrary.Sites;

    public static class WikiWordDownloaderFactory
    {
        public static IObservable<WikiPage> CreateCategoryDownloader(WikiSite site, string categoryTitle)
        {
            var page = new WikiPage(site, categoryTitle);
            var generator = new CategoryMembersGenerator(page);
            var observable = generator.EnumPagesAsync()
                .Where((page, b) => !page.IsSpecialPage)
                .ToObservable();

            return observable;
        }

        public static (IObservable<WikiPage> AdjectiveDownloader, IObservable<WikiPage> NounDownloader) CreateLanguageDownloaders(WikiSite site, Language language)
        {
            var adjectiveDownloader = CreateCategoryDownloader(site, language.AdjectiveCategoryTitle);
            var nounDownloader = CreateCategoryDownloader(site, language.NounCategoryTitle);

            return (adjectiveDownloader, nounDownloader);
        }
    }
}