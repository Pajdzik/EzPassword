namespace EzPassword.Core
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using WikiClientLibrary;
    using WikiClientLibrary.Client;
    using WikiClientLibrary.Generators;

    public class NounGenerator
    {
        private const string ClientUserAgent = "EzPassword 0.0.1";

        private readonly WikiClient wikiClient;
        private readonly string apiEndpoint;

        public NounGenerator(string apiEndpoint)
        {
            this.apiEndpoint = apiEndpoint;
            this.wikiClient = new WikiClient()
            {
                ClientUserAgent = ClientUserAgent
            };
        }

        public async Task<string> GetRandomNoun()
        {
            Site site = await Site.CreateAsync(this.wikiClient, this.apiEndpoint);
            //Page page = new Page(site, );
            var categoryMembersGenerator = new CategoryMembersGenerator(site, "Kategoria:Język polski - przymiotniki")
            {
                MemberTypes = CategoryMemberTypes.Page
            };

            var pages = categoryMembersGenerator.EnumPages().Skip(3004).Take(10).ToList();

            return null;
        }
    }
}