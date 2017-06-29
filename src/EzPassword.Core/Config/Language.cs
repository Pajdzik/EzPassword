namespace EzPassword.Core.Config
{
    using System;
    using System.Threading.Tasks;
    using WikiClientLibrary;
    using WikiClientLibrary.Client;
    using WikiClientLibrary.Generators;

    public struct Language
    {
        private readonly Site site;

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

        public CategoryMembersGenerator AdjectiveCategory
        {
            get
            {
                return new CategoryMembersGenerator(this.site, this.AdjectiveCategoryTitle)
                {
                    MemberTypes = CategoryMemberTypes.Page
                };
            }
        }

        public CategoryMembersGenerator NounCategory
        {
            get
            {
                return new CategoryMembersGenerator(this.site, this.NounCategoryTitle)
                {
                    MemberTypes = CategoryMemberTypes.Page
                };
            }
        }

        internal static Language Create(string symbol, string wikiApiUrl, string adjectiveCategoryUrl,
            string nounCategoryUrl)
        {
            return new Language(symbol,
                new Uri(wikiApiUrl),
                adjectiveCategoryUrl,
                nounCategoryUrl);
        }

        private Site CreateSite(string wikiApiUrl)
        {
            Site site = null;

            Task.Run(async () =>
            {
                var wikiClient = new WikiClient();
                site = await Site.CreateAsync(wikiClient, wikiApiUrl);
            }).Wait();

            return site;
        }
    }
}