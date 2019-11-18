namespace EzPassword.Core.Wiki
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NLog;
    using WikiClientLibrary.Pages;

    public class TextFileWordPersister : IWordPersister
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly string directoryPath;
        private readonly string fileNameTemplate;
        private readonly IDictionary<int, StreamWriter> streamWriters;

        private string lastFirstTwoLetters = String.Empty;

        public TextFileWordPersister(string directoryPath, string fileNameTemplate)
        {
            this.directoryPath = directoryPath;
            this.fileNameTemplate = fileNameTemplate;
            this.streamWriters = new Dictionary<int, StreamWriter>();

            CreateDirectory(directoryPath);
        }

        public void OnNext(WikiPage page)
        {
            string value = page.Title;
            string firstTwoLetters = value.Substring(0, Math.Min(2, value.Length));
            if (!String.Equals(this.lastFirstTwoLetters, firstTwoLetters, StringComparison.InvariantCultureIgnoreCase))
            {
                this.lastFirstTwoLetters = firstTwoLetters;
                Logger.Info(firstTwoLetters);
            }

            Logger.Debug($"Saving {value}");
            StreamWriter streamWriter = this.GetStreamWriter(value.Length, this.directoryPath, this.fileNameTemplate);
            streamWriter.WriteLine(value);
        }

        public void OnError(Exception error)
        {
            Logger.Error(error);
        }

        public void OnCompleted()
        {
            Logger.Debug("Disposing StreamWriters");
            foreach (var streamWriter in this.streamWriters.Values)
            {
                streamWriter.Dispose();
            }

            Logger.Debug("Completed!");
        }

        private static void CreateDirectory(string directoryPath)
        {
            Logger.Info($"Creating directory \"{directoryPath}\"");
            Directory.CreateDirectory(directoryPath);
        }

        private StreamWriter GetStreamWriter(int wordLength, string directoryPath, string fileNameTemplate)
        {
            if (!this.streamWriters.ContainsKey(wordLength))
            {
                Logger.Debug($"Creating StreamWriter for length {wordLength}");
                var fileName = string.Format($"{directoryPath}/{fileNameTemplate}", wordLength);
                this.streamWriters[wordLength] = File.AppendText(fileName);
            }

            return this.streamWriters[wordLength];
        }
    }
}