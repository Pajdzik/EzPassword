namespace EzPassword.Core.Config
{
    using System;

    public class Language
    {
        public Language(string symbol, Uri wikiApi, string adjectiveCategoryTitle, string nounCategoryTitle)
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

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Language))
            {
                return false;
            }

            var otherLanguage = (Language)obj;

            return this.Symbol.Equals(otherLanguage.Symbol, StringComparison.InvariantCultureIgnoreCase)
                && this.WikiApi.Equals(otherLanguage.WikiApi)
                && this.AdjectiveCategoryTitle.Equals(otherLanguage.AdjectiveCategoryTitle, StringComparison.InvariantCultureIgnoreCase)
                && this.NounCategoryTitle.Equals(otherLanguage.NounCategoryTitle, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 911;
                hash = (hash * 29) + this.Symbol.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
                hash = (hash * 29) + this.WikiApi.GetHashCode();
                hash = (hash * 29) + this.AdjectiveCategoryTitle.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
                hash = (hash * 29) + this.NounCategoryTitle.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
                return hash;
            }
        }
    }
}