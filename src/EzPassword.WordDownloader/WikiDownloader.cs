namespace EzPassword.WordDownloader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NLog;
    using WikiClientLibrary.Generators;

    internal class WikiDownloader
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly CategoryMembersGenerator categoryMembersGenerator;
        private readonly IDictionary<int, StreamWriter> streamWriters;

        public WikiDownloader(CategoryMembersGenerator categoryMembersGenerator)
        {
            this.categoryMembersGenerator = categoryMembersGenerator;
            this.streamWriters = new Dictionary<int, StreamWriter>();
        }

        public void DownloadWords(string directoryPath, string fileNameTemplate)
        {
            logger.Info($"Creating directory \"{directoryPath}\"");
            Directory.CreateDirectory(directoryPath);

            logger.Info($"Downloading pages from \"{this.categoryMembersGenerator.CategoryTitle}\"");
            this.categoryMembersGenerator.EnumPagesAsync().Take(500).ForEach((page, i) =>
            {
                logger.Debug($"Saving {page.Title}");
                var streamWriter = this.GetStreamWriter(page.Title.Length, directoryPath, fileNameTemplate);
                streamWriter.WriteLineAsync(page.Title).Wait();
            });

            logger.Debug("Disposing StreamWriters");
            foreach (var streamWriter in this.streamWriters.Values)
            {
                streamWriter.Dispose();
            }
        }

        private StreamWriter GetStreamWriter(int wordLength, string directoryPath, string fileNameTemplate)
        {
            if (!this.streamWriters.ContainsKey(wordLength))
            {
                logger.Debug($"Creating StreamWriter for length {wordLength}");
                var fileName = string.Format($"{directoryPath}/{fileNameTemplate}", wordLength);
                this.streamWriters[wordLength] = File.AppendText(fileName);
            }

            return this.streamWriters[wordLength];
        }
    }
}