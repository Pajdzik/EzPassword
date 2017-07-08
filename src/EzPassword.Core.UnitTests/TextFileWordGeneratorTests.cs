namespace EzPassword.Core.UnitTests
{
    using System;
    using Eks.Abstraction.System.IO;
    using NSubstitute;
    using Xunit;

    public class TextFileWordGeneratorTests
    {
        [Fact]
        public void Ctor_ThrowsException_WhenNullDirectoryProxyPassed()
        {
            Assert.Throws<ArgumentNullException>(() => new TextFileWordGenerator(null, @"C:\temp", "template.txt"));
        }

        [Fact]
        public void Ctor_ThrowsException_WhenNullAdjectiveDirectoryPathPassed()
        {
            var directoryProxyMock = Substitute.For<IDirectoryProxy>();
            Assert.Throws<ArgumentNullException>(() => new TextFileWordGenerator(directoryProxyMock, null, "template.txt"));
        }

        [Fact]
        public void Ctor_ThrowsException_WhenNullTemplatePassed()
        {
            var directoryProxyMock = Substitute.For<IDirectoryProxy>();
            Assert.Throws<ArgumentNullException>(() => new TextFileWordGenerator(directoryProxyMock, @"C:\temp", null));
        }

        [Fact]
        public void Ctor_Foo()
        {
            const string wordDirectoryPath = @"C:\temp";

            var directoryProxyMock = Substitute.For<IDirectoryProxy>();
            directoryProxyMock.Exists(wordDirectoryPath).Returns(true);
            directoryProxyMock.GetFiles(wordDirectoryPath).Returns(new [] { @"C:\temp\nouns_02.txt" });

            var a = new TextFileWordGenerator(directoryProxyMock, wordDirectoryPath, @"nouns_(\d+).txt");
        }
    }
}