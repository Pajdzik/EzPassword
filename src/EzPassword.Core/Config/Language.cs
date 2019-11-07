namespace EzPassword.Core.Config
{
    using System;
    using System.Collections.Generic;
    using WikiClientLibrary.Client;
    using WikiClientLibrary.Generators;
    using WikiClientLibrary.Sites;

    public struct Language : IEquatable<Language>
    {
        private readonly WikiSite site;

        public Language(string symbol, Uri wikiApi, string adjectiveCategoryTitle, string nounCategoryTitle) : this()
        {
            this.Symbol = symbol;
            this.WikiApi = wikiApi;
            this.AdjectiveCategoryTitle = adjectiveCategoryTitle;
            this.NounCategoryTitle = nounCategoryTitle;

            if (this.WikiApi == null)
            {
                throw new ArgumentNullException($"{nameof(this.WikiApi)} cannot be null");
            }

            this.site = this.CreateSite(this.WikiApi.AbsoluteUri);
        }

        public string Symbol { get; }

        public Uri WikiApi { get; }

        public string AdjectiveCategoryTitle { get; }

        public string NounCategoryTitle { get; }

        public readonly CategoryMembersGenerator AdjectiveCategory =>
            new CategoryMembersGenerator(this.site, this.AdjectiveCategoryTitle)
            {
                MemberTypes = CategoryMemberTypes.Page
            };

        public readonly CategoryMembersGenerator NounCategory =>
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

        public override bool Equals(object obj)
        {
            return obj != null && obj is Language && this.Equals((Language)obj);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(site, Symbol, WikiApi, AdjectiveCategoryTitle, NounCategoryTitle, AdjectiveCategory, NounCategory);
        }

        private WikiSite CreateSite(string wikiApiUrl)
        {
            using var wikiClient = new WikiClient();
            var site = new WikiSite(wikiClient, wikiApiUrl);
            site.Initialization.Wait();
            return site;
        }

        public static bool operator ==(Language left, Language right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Language left, Language right)
        {
            return !(left == right);
        }

        public bool Equals(Language other)
        {
            return EqualityComparer<WikiSite>.Default.Equals(site, other.site) &&
                   Symbol == other.Symbol &&
                   EqualityComparer<Uri>.Default.Equals(WikiApi, other.WikiApi) &&
                   AdjectiveCategoryTitle == other.AdjectiveCategoryTitle &&
                   NounCategoryTitle == other.NounCategoryTitle &&
                   EqualityComparer<CategoryMembersGenerator>.Default.Equals(AdjectiveCategory, other.AdjectiveCategory) &&
                   EqualityComparer<CategoryMembersGenerator>.Default.Equals(NounCategory, other.NounCategory);
        }
    }
}