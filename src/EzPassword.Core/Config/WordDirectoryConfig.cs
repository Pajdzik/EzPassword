namespace EzPassword.Core.Config
{
    public static class WordDirectoryConfig
    {
        public static string AdjectiveDirectoryName => "adjectives";

        public static string AdjectiveFileNameTemplate => "adjectives_{0:D2}.txt";

        public static string AdjectiveFileNameRegex => "adjectives_(\\d+).txt";

        public static string NounDirectoryName => "nouns";

        public static string NounFileNameTemplate => "nouns_{0:D2}.txt";

        public static string NounFileNameRegex => "nouns_(\\d+).txt";

    }
}