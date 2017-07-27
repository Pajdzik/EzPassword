namespace EzPassword.Core.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public class PasswordGeneratingChainTests
    {
        public class Ctor
        {
            [Fact]
            public void ThrowsException_WhenNullCollectionPassed()
            {
                Action construct = () => new PasswordGeneratingChain(null);
                construct.ShouldThrow<ArgumentNullException>();
            }
        }

        public class Generate
        {
            [Theory]
            [InlineData("", new[] { "" })]
            [InlineData("a", new[] { "a" })]
            [InlineData("ab", new[] { "a", "b" })]
            [InlineData("aabbcc", new[] { "aa", "bb", "cc" })]
            public void ReturnsProperPassword_ForWorkingGenerators(string expectedPassword, string[] generatedWords)
            {
                IEnumerable<Func<string>> generators = generatedWords.Select(word => (Func<string>)(() => word));
                var passwordGeneratingChain = new PasswordGeneratingChain(generators.ToArray());

                Password password = passwordGeneratingChain.Generate();

                password.ToString().Should().Be(expectedPassword);
            }
        }
    }
}
