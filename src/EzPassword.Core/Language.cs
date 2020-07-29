namespace EzPassword.Core
{
    using Kpax.Core.Collections;

    public enum Language
    {
        Unknown,
        English,
        Polish,
    }

    public static class LanguageSymbol
    {
        private static TwoWayDictionary<Language, string> mapping = new TwoWayDictionary<Language, string>()
        {
            { Language.English, "en" },
            { Language.Polish, "pl" },
        };

        public static string Get(Language language)
        {
            return mapping[language];
        }

        public static Language Get(string language)
        {
            return mapping[language];
        }
    }
}
