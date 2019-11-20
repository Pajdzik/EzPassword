namespace EzPassword.Core.Wiki
{
    using System;
    using System.Linq;
    using EzPassword.Core.Config;
    using WikiClientLibrary.Generators;
    using WikiClientLibrary.Pages;
    using WikiClientLibrary.Sites;

    public class WikiWordDownloaderFactory
    {
        public IObservable<WikiPage> CreateCategoryDownloader(WikiSite site, string categoryTitle)
        {
            var page = new WikiPage(site, categoryTitle);
            var generator = new CategoryMembersGenerator(page);
            var observable = generator.EnumPagesAsync()
                .Where((page, b) => !page.IsSpecialPage)
                .ToObservable();
            
            return observable;
        }

        public (IObservable<WikiPage>, IObservable<WikiPage>) CreateLanguageDownloaders(WikiSite site, Language language)
        {
            var adjectiveDownloader = this.CreateCategoryDownloader(site, language.AdjectiveCategoryTitle);
            var nounDownloader = this.CreateCategoryDownloader(site, language.NounCategoryTitle);

            return (adjectiveDownloader, nounDownloader);
        }
    }
}