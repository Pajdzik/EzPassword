namespace EzPassword.Core.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Eks.Abstraction.System.IO;
    using FluentAssertions;
    using NSubstitute;
    using Xunit;

    public class TextFileWordGeneratorTests
    {
        private const string WordDirectoryPath = @"C:\temp";
        private const string FileNameRegex = @"nouns_(\d+).txt";

        public class Ctor
        {
            [Fact]
            public void ThrowsException_WhenNullDirectoryProxyPassed()
            {
                var fileProxyMock = Substitute.For<IFileFacade>();
                Action construct = () => new TextFileWordGenerator(null, fileProxyMock, @"C:\temp", "template.txt");
                construct.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void ThrowsException_WhenNullAdjectiveDirectoryPathPassedAsync()
            {
                var directoryProxyMock = Substitute.For<IDirectoryFacade>();
                var fileProxyMock = Substitute.For<IFileFacade>();
                Action construct = () => new TextFileWordGenerator(directoryProxyMock, fileProxyMock, null, "template.txt");
                construct.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void ThrowsException_WhenNullTemplatePassed()
            {
                var directoryProxyMock = Substitute.For<IDirectoryFacade>();
                var fileProxyMock = Substitute.For<IFileFacade>();
                Action construct = () =>
                    new TextFileWordGenerator(directoryProxyMock, fileProxyMock, @"C:\temp", null);
                construct.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void ThrowsException_WhenNullFileFacadePassed()
            {
                var fileProxyMock = Substitute.For<IFileFacade>();
                Action construct = () =>
                    new TextFileWordGenerator(null, fileProxyMock, @"C:\temp", "template.txt");
                construct.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void ThrowsException_WhenDirectoryDoesntExist()
            {
                const string WordDirectoryPath = @"C:\temp";

                var directoryProxyMock = Substitute.For<IDirectoryFacade>();
                directoryProxyMock.Exists(WordDirectoryPath).Returns(false);

                var fileProxyMock = Substitute.For<IFileFacade>();

                Action construct = () =>
                    new TextFileWordGenerator(directoryProxyMock, fileProxyMock, WordDirectoryPath, FileNameRegex);
                construct.ShouldThrow<ArgumentException>();
            }

            [Fact]
            public void ThrowsException_WhenNoFilesAreAvailable()
            {
                const string WordDirectoryPath = @"C:\temp";

                var directoryProxyMock = Substitute.For<IDirectoryFacade>();
                directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new string[0]);
                directoryProxyMock.Exists(WordDirectoryPath).Returns(true);

                var fileProxyMock = Substitute.For<IFileFacade>();

                Action construct = () =>
                    new TextFileWordGenerator(directoryProxyMock, fileProxyMock, WordDirectoryPath, FileNameRegex);
                construct.ShouldThrow<ArgumentException>();
            }

            [Fact]
            public void ThrowsException_WhenEmptyFilesArePassed()
            {
                const string WordDirectoryPath = @"C:\temp";

                var directoryProxyMock = Substitute.For<IDirectoryFacade>();
                directoryProxyMock.GetFiles(WordDirectoryPath).Returns(new[] { @"C:\temp\nouns_01.txt" });
                directoryProxyMock.Exists(WordDirectoryPath).Returns(true);

                var fileProxyMock = Substitute.For<IFileFacade>();
                fileProxyMock.ReadAllLines(WordDirectoryPath).Returns(new string[0]);

                Action construct = () =>
                    new TextFileWordGenerator(directoryProxyMock, fileProxyMock, WordDirectoryPath, FileNameRegex);
                construct.ShouldThrow<ArgumentException>();
            }
        }

        public class ShortestWordLength
        {
            private readonly IDirectoryFacade directoryProxyMock;
            private readonly IFileFacade fileProxyMock;

            public ShortestWordLength()
            {
                this.directoryProxyMock = Substitute.For<IDirectoryFacade>();
                this.directoryProxyMock.Exists(WordDirectoryPath).Returns(true);

                this.fileProxyMock = Substitute.For<IFileFacade>();
            }

            [Fact]
            public void ReturnsCorrectValue_WhenOneLengthPassed()
            {
                const string FilePath = @"C:\temp\nouns_01.txt";
                this.directoryProxyMock.GetFiles(WordDirectoryPath).Returns(new[] { FilePath });
                this.fileProxyMock.ReadAllLines(FilePath).Returns(new[] { "a" });

                var textFileWordGenerator = new TextFileWordGenerator(
                    this.directoryProxyMock,
                    this.fileProxyMock,
                    WordDirectoryPath,
                    FileNameRegex);

                textFileWordGenerator.ShortestWordLength.Should().Be(1);
            }

            [Fact]
            public void ReturnsCorrectValue_WhenTwoLengthsPassed()
            {
                string[] FilePaths = new[] { @"C:\temp\nouns_02.txt", @"C:\temp\nouns_30.txt" };
                this.directoryProxyMock.GetFiles(WordDirectoryPath).Returns(FilePaths);
                this.fileProxyMock.ReadAllLines(FilePaths[0]).Returns(new[] { "ab" });
                this.fileProxyMock.ReadAllLines(FilePaths[1]).Returns(Enumerable.Repeat("a", 30).ToArray());

                var textFileWordGenerator = new TextFileWordGenerator(
                    this.directoryProxyMock,
                    this.fileProxyMock,
                    WordDirectoryPath,
                    FileNameRegex);

                textFileWordGenerator.ShortestWordLength.Should().Be(2);
            }
        }

        public class LongestWordLength
        {
            private readonly IDirectoryFacade directoryProxyMock;
            private readonly IFileFacade fileProxyMock;

            public LongestWordLength()
            {
                this.directoryProxyMock = Substitute.For<IDirectoryFacade>();
                this.directoryProxyMock.Exists(WordDirectoryPath).Returns(true);

                this.fileProxyMock = Substitute.For<IFileFacade>();
            }

            [Fact]
            public void ReturnsCorrectValue_WhenOneLengthPassed()
            {
                const string FilePath = @"C:\temp\nouns_01.txt";
                this.directoryProxyMock.GetFiles(WordDirectoryPath).Returns(new[] { FilePath });
                this.fileProxyMock.ReadAllLines(FilePath).Returns(new[] { "a" });

                var textFileWordGenerator = new TextFileWordGenerator(
                    this.directoryProxyMock,
                    this.fileProxyMock,
                    WordDirectoryPath,
                    FileNameRegex);

                textFileWordGenerator.LongestWordLength.Should().Be(1);
            }

            [Fact]
            public void ReturnsCorrectValue_WhenTwoLengthsPassed()
            {
                string[] FilePaths = new[] { @"C:\temp\nouns_02.txt", @"C:\temp\nouns_30.txt" };
                this.directoryProxyMock.GetFiles(WordDirectoryPath).Returns(FilePaths);
                this.fileProxyMock.ReadAllLines(FilePaths[0]).Returns(new[] { "ab" });
                this.fileProxyMock.ReadAllLines(FilePaths[1]).Returns(Enumerable.Repeat("a", 30).ToArray());

                var textFileWordGenerator = new TextFileWordGenerator(
                    this.directoryProxyMock,
                    this.fileProxyMock,
                    WordDirectoryPath,
                    FileNameRegex);

                textFileWordGenerator.LongestWordLength.Should().Be(30);
            }
        }

        public class GetRandomWordInt
        {
            private readonly IDirectoryFacade directoryProxyMock;
            private readonly IFileFacade fileProxyMock;

            public GetRandomWordInt()
            {
                this.directoryProxyMock = Substitute.For<IDirectoryFacade>();
                this.directoryProxyMock.Exists(WordDirectoryPath).Returns(true);

                this.fileProxyMock = Substitute.For<IFileFacade>();
            }

            [Fact]
            public void ReturnsSameWord_WhenSingleWordPassed_AndProperLengthPassed()
            {
                const string Word = "a";

                this.directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new[] { @"C:\temp\nouns_01.txt" });
                this.fileProxyMock.ReadAllLines(Arg.Any<string>()).Returns(new[] { Word });

                var textFileWordGenerator = new TextFileWordGenerator(
                    this.directoryProxyMock, 
                    this.fileProxyMock,
                    WordDirectoryPath, 
                    FileNameRegex);

                string result = textFileWordGenerator.GetRandomWord(1);

                result.Should().Be(Word);
            }

            [Fact]
            public void ReturnsWordLength1_WhenTwoWordsPassed_AndProperLengthPassed()
            {
                const string Word = "a";

                this.directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new[] { @"C:\temp\nouns_01.txt", @"C:\temp\nouns_02.txt" });
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_01.txt").Returns(new[] { Word });
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_02.txt").Returns(new[] { Word + Word });

                var textFileWordGenerator = new TextFileWordGenerator(
                    this.directoryProxyMock, 
                    this.fileProxyMock,
                    WordDirectoryPath, 
                    FileNameRegex);

                string result = textFileWordGenerator.GetRandomWord(1);

                result.Should().Be(Word);
            }

            [Fact]
            public void ReturnsWordLength2_WhenTwoWordsPassed_AndProperLengthPassed()
            {
                const string Word = "a";

                this.directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new[] { @"C:\temp\nouns_01.txt", @"C:\temp\nouns_02.txt" });
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_01.txt").Returns(new[] { Word });
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_02.txt").Returns(new[] { Word + Word });

                var textFileWordGenerator = new TextFileWordGenerator(
                    this.directoryProxyMock, 
                    this.fileProxyMock,
                    WordDirectoryPath, 
                    FileNameRegex);

                string result = textFileWordGenerator.GetRandomWord(2);

                result.Should().Be(Word + Word);
            }

            [Fact]
            public void ThrowsException_WhenSingleWordPassed_ButWrongLengthPassed()
            {
                const string Word = "a";

                this.directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new[] { @"C:\temp\nouns_01.txt" });
                this.fileProxyMock.ReadAllLines(Arg.Any<string>()).Returns(new string[] { Word });

                var textFileWordGenerator = new TextFileWordGenerator(
                    this.directoryProxyMock, 
                    this.fileProxyMock,
                    WordDirectoryPath, 
                    FileNameRegex);

                Action action = () => textFileWordGenerator.GetRandomWord(2);

                action.ShouldThrow<ArgumentOutOfRangeException>();
            }
        }
        
        public class GetRandomWord
        {
            private readonly IDirectoryFacade directoryProxyMock;
            private readonly IFileFacade fileProxyMock;

            public GetRandomWord()
            {
                this.directoryProxyMock = Substitute.For<IDirectoryFacade>();
                this.directoryProxyMock.Exists(WordDirectoryPath).Returns(true);

                this.fileProxyMock = Substitute.For<IFileFacade>();
            }

            [Fact]
            public void ReturnsAllWords_WhenRangeIsNotContinuous()
            {
                var words1 = new HashSet<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u" };
                var words3 = new HashSet<string> { "mos", "soh", "exr", "win", "jeh", "sra", "edp", "git", "jot", "kip", "ana", "dau", "pta", "tay", "col", "bem", "ace", "sfz", "nga", "apa" };
                var words5 = new HashSet<string> { "reign", "joyce", "marat", "oxime", "antal", "chufa", "upper", "kalis", "jurez", "jeeps", "pella", "sigyn", "bases", "pilar", "smell", "rival", "italo", "pleat", "gulag", "hodge" };

                this.directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new[] { @"C:\temp\nouns_01.txt", @"C:\temp\nouns_03.txt", @"C:\temp\nouns_05.txt" });
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_01.txt").Returns(words1.ToArray());
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_03.txt").Returns(words3.ToArray());
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_05.txt").Returns(words5.ToArray());

                var textFileWordGenerator = new TextFileWordGenerator(
                    this.directoryProxyMock, 
                    this.fileProxyMock,
                    WordDirectoryPath, 
                    FileNameRegex);

                int count = words1.Count + words3.Count + words5.Count;
                var results = new HashSet<string>();

                for (int i = 0; i < count * 10; i++)
                {
                    string result = textFileWordGenerator.GetRandomWord();
                    results.Add(result);
                }

                results.Count.Should().Be(count);
            }

            [Fact]
            public void ReturnsAllWords_WhenRangeIsContinuous()
            {
                var words1 = new HashSet<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u" };
                var words2 = new HashSet<string> { "is", "dj", "in", "go", "an", "am", "no", "ml", "ka", "ic", "up", "be", "if", "so", "at", "it", "io", "me", "or", "he" };
                var words3 = new HashSet<string> { "mos", "soh", "exr", "win", "jeh", "sra", "edp", "git", "jot", "kip", "ana", "dau", "pta", "tay", "col", "bem", "ace", "sfz", "nga", "apa" };
                var words4 = new HashSet<string> { "hadj", "thor", "niff", "quot", "ford", "crag", "dato", "bine", "comm", "nirc", "rudd", "cass", "hurt", "poet", "bare", "neap", "trew", "tank", "dawk", "sore" };
                var words5 = new HashSet<string> { "reign", "joyce", "marat", "oxime", "antal", "chufa", "upper", "kalis", "jurez", "jeeps", "pella", "sigyn", "bases", "pilar", "smell", "rival", "italo", "pleat", "gulag", "hodge" };

                this.directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new[] { @"C:\temp\nouns_01.txt", @"C:\temp\nouns_02.txt", @"C:\temp\nouns_03.txt", @"C:\temp\nouns_04.txt", @"C:\temp\nouns_05.txt" });
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_01.txt").Returns(words1.ToArray());
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_02.txt").Returns(words2.ToArray());
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_03.txt").Returns(words3.ToArray());
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_04.txt").Returns(words4.ToArray());
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_05.txt").Returns(words5.ToArray());

                var textFileWordGenerator = new TextFileWordGenerator(
                    this.directoryProxyMock, 
                    this.fileProxyMock,
                    WordDirectoryPath, 
                    FileNameRegex);

                int count = words1.Count + words2.Count + words3.Count + words4.Count + words5.Count;
                var results = new HashSet<string>();

                for (int i = 0; i < count * 10; i++)
                {
                    string result = textFileWordGenerator.GetRandomWord();
                    results.Add(result);
                }

                results.Count.Should().Be(count);
            }
        }
    }
}