namespace EzPassword.Core.Config
{
    using System;
    using System.Threading.Tasks;
    using WikiClientLibrary.Client;
    using WikiClientLibrary.Generators;
    using WikiClientLibrary.Sites;

    public struct Language
    {
        private readonly WikiSite site;

        public Language(string symbol, Uri wikiApi, string adjectiveCategoryTitle, string nounCategoryTitle) : this()
        {
            this.Symbol = symbol;
            this.WikiApi = wikiApi;
            this.AdjectiveCategoryTitle = adjectiveCategoryTitle;
            this.NounCategoryTitle = nounCategoryTitle;

            this.site = this.CreateSite(this.WikiApi.AbsoluteUri);
        }

        public string Symbol { get; }

        public Uri WikiApi { get; }

        public string AdjectiveCategoryTitle { get; }

        public string NounCategoryTitle { get; }

        public CategoryMembersGenerator AdjectiveCategory =>
            new CategoryMembersGenerator(this.site, this.AdjectiveCategoryTitle)
            {
                MemberTypes = CategoryMemberTypes.Page
            };

        public CategoryMembersGenerator NounCategory =>
            new CategoryMembersGenerator(this.site, this.NounCategoryTitle)
            {
                MemberTypes = CategoryMemberTypes.Page
            };

        internal static Language Create(string symbol, string wikiApiUrl, string adjectiveCategoryUrl, string nounCategoryUrl)
        {
            return new Language(
                symbol,
                new Uri(wikiApiUrl),
                adjectiveCategoryUrl,
                nounCategoryUrl);
        }

        private WikiSite CreateSite(string wikiApiUrl)
        {
            var wikiClient = new WikiClient();
            var site = new WikiSite(wikiClient, wikiApiUrl);

            return site;
        }
    }
}