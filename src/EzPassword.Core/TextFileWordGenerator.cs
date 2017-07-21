namespace EzPassword.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Eks.Abstraction.System.IO;

    public class TextFileWordGenerator : IRandomWordGenerator
    {
        private static readonly Random random = new Random();

        private readonly Dictionary<int, string[]> wordFiles;

        public TextFileWordGenerator(IDirectoryFacade directory, IFileFacade file, string wordDirectoryPath, string fileNameRegex)
        {
            if (!directory.Exists(wordDirectoryPath))
            {
                throw new ArgumentException(nameof(wordDirectoryPath));
            }

            string[] files = directory.GetFiles(wordDirectoryPath);
            this.wordFiles = files.ToDictionary(f => GetWordLength(f, fileNameRegex), file.ReadAllLines);
        }

        public string GetRandomWord()
        {
            var keys = this.wordFiles.Keys;
            int randomIndex = random.Next(keys.Count);

            return this.GetRandomWord(keys.ElementAt(randomIndex));
        }

        public string GetRandomWord(int wordLength)
        {
            return GetRandomWord(this.wordFiles, wordLength);
        }

        public static string GetRandomWord(IDictionary<int, string[]> wordsByLength, int wordLength)
        {
            if (!wordsByLength.ContainsKey(wordLength))
            {
                throw new ArgumentOutOfRangeException(nameof(wordLength));
            }

            string[] words = wordsByLength[wordLength];
            int randomIndex = random.Next(words.Length);

            return words[randomIndex];
        }

        private static int GetWordLength(string fileName, string fileNameRegex)
        {
            string fileNameWithoutExtension = Path.GetFileName(fileName);
            Match match = Regex.Match(fileNameWithoutExtension, fileNameRegex);

            return int.Parse(match.Groups[1].Value);
        }
    }
}