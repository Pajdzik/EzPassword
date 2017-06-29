namespace EzPassword.WordDownloader
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using WikiClientLibrary.Generators;

    internal class WikiDownloader
    {
        private readonly CategoryMembersGenerator categoryMembersGenerator;
        private readonly IDictionary<int, StreamWriter> streamWriters;

        public WikiDownloader(CategoryMembersGenerator categoryMembersGenerator)
        {
            this.categoryMembersGenerator = categoryMembersGenerator;
            this.streamWriters = new Dictionary<int, StreamWriter>();
        }

        public void DownloadWords(string directoryPath, string fileNameTemplate)
        {
            Directory.CreateDirectory(directoryPath);

            this.categoryMembersGenerator.EnumPagesAsync().Take(500).ForEach((page, i) =>
            {
                var streamWriter = this.GetStreamWriter(page.Title.Length, directoryPath, fileNameTemplate);
                streamWriter.WriteLineAsync(page.Title).Wait();
            });

            foreach (var streamWriter in this.streamWriters.Values)
            {
                streamWriter.Dispose();
            }
        }

        private StreamWriter GetStreamWriter(int wordLength, string directoryPath, string fileNameTemplate)
        {
            if (!this.streamWriters.ContainsKey(wordLength))
            {
                var fileName = string.Format($"{directoryPath}/{fileNameTemplate}", wordLength);
                this.streamWriters[wordLength] = File.AppendText(fileName);
            }

            return this.streamWriters[wordLength];
        }
    }
}