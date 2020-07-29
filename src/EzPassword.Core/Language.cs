namespace EzPassword.Core
{
    using Kpax.Core.Collections;

    public static class Language
    {
        private static TwoWayDictionary<Name, string> mapping = new TwoWayDictionary<Name, string>()
        {
            { Name.English, "en" },
            { Name.Polish, "pl" },
        };

        public static string Get(Name language)
        {
            return mapping[language];
        }

        public static Name Get(string language)
        {
            return mapping[language];
        }

        public enum Name
        {
            Unknown,
            English,
            Polish,
        }
    }
}
