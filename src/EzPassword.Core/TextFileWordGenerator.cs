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
        private readonly Dictionary<int, string[]> wordFiles;

        public TextFileWordGenerator(IDirectoryFacade directory, string wordDirectoryPath, string fileNameRegex)
        {
            if (!directory.Exists(wordDirectoryPath))
            {
                throw new ArgumentException(nameof(wordDirectoryPath));
            }

            string[] files = directory.GetFiles(wordDirectoryPath);
            this.wordFiles = files.ToDictionary(file => GetWordLength(file, fileNameRegex), file => File.ReadAllLines(file));
        }

        public string GetRandomWord()
        {
            throw new System.NotImplementedException();
        }

        public string GetRandomWord(int wordLength)
        {
            throw new NotImplementedException();
        }

        private static int GetWordLength(string fileName, string fileNameRegex)
        {
            string fileNameWithoutExtension = Path.GetFileName(fileName);
            Match match = Regex.Match(fileNameWithoutExtension, fileNameRegex);

            return int.Parse(match.Groups[1].Value);
        }
    }
}