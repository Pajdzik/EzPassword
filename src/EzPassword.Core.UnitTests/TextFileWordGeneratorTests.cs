﻿namespace EzPassword.Core.UnitTests
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
                const string wordDirectoryPath = @"C:\temp";

                var directoryProxyMock = Substitute.For<IDirectoryFacade>();
                directoryProxyMock.Exists(wordDirectoryPath).Returns(false);

                var fileProxyMock = Substitute.For<IFileFacade>();

                Action construct = () =>
                    new TextFileWordGenerator(directoryProxyMock, fileProxyMock, wordDirectoryPath, FileNameRegex);
                construct.ShouldThrow<ArgumentException>();
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
                const string word = "a";

                this.directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new[] {@"C:\temp\nouns_01.txt"});
                this.fileProxyMock.ReadAllLines(Arg.Any<string>()).Returns(new[] {word});

                var textFileWordGenerator = new TextFileWordGenerator(this.directoryProxyMock, this.fileProxyMock,
                    WordDirectoryPath, FileNameRegex);

                string result = textFileWordGenerator.GetRandomWord(1);

                result.Should().Be(word);
            }

            [Fact]
            public void ReturnsWordLength1_WhenTwoWordsPassed_AndProperLengthPassed()
            {
                const string word = "a";

                this.directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new[] { @"C:\temp\nouns_01.txt", @"C:\temp\nouns_02.txt" });
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_01.txt").Returns(new[] { word });
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_02.txt").Returns(new[] { word + word });

                var textFileWordGenerator = new TextFileWordGenerator(this.directoryProxyMock, this.fileProxyMock,
                    WordDirectoryPath, FileNameRegex);

                string result = textFileWordGenerator.GetRandomWord(1);

                result.Should().Be(word);
            }

            [Fact]
            public void ReturnsWordLength2_WhenTwoWordsPassed_AndProperLengthPassed()
            {
                const string word = "a";

                this.directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new[] { @"C:\temp\nouns_01.txt", @"C:\temp\nouns_02.txt" });
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_01.txt").Returns(new[] { word });
                this.fileProxyMock.ReadAllLines(@"C:\temp\nouns_02.txt").Returns(new[] { word + word });

                var textFileWordGenerator = new TextFileWordGenerator(this.directoryProxyMock, this.fileProxyMock,
                    WordDirectoryPath, FileNameRegex);

                string result = textFileWordGenerator.GetRandomWord(2);

                result.Should().Be(word + word);
            }

            [Fact]
            public void ThrowsException_WhenSingleWordPassed_ButWrongLengthPassed()
            {
                const string word = "a";

                this.directoryProxyMock.GetFiles(Arg.Any<string>()).Returns(new[] { @"C:\temp\nouns_01.txt" });
                this.fileProxyMock.ReadAllLines(Arg.Any<string>()).Returns(new string[] { word });

                var textFileWordGenerator = new TextFileWordGenerator(this.directoryProxyMock, this.fileProxyMock,
                    WordDirectoryPath, FileNameRegex);

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

                var textFileWordGenerator = new TextFileWordGenerator(this.directoryProxyMock, this.fileProxyMock,
                    WordDirectoryPath, FileNameRegex);

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

                var textFileWordGenerator = new TextFileWordGenerator(this.directoryProxyMock, this.fileProxyMock,
                    WordDirectoryPath, FileNameRegex);

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