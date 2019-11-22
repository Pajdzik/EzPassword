namespace EzPassword.Core.Config
{
    using System;

    public struct Language
    {
        public Language(string symbol, Uri wikiApi, string adjectiveCategoryTitle, string nounCategoryTitle) : this()
        {
            this.Symbol = symbol;
            this.WikiApi = wikiApi;
            this.AdjectiveCategoryTitle = adjectiveCategoryTitle;
            this.NounCategoryTitle = nounCategoryTitle;
        }

        public string Symbol { get; }

        public Uri WikiApi { get; }

        public string AdjectiveCategoryTitle { get; }

        public string NounCategoryTitle { get; }
    }
}