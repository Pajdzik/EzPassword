namespace EzPassword.Core.UnitTests
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using Xunit;

    public sealed class PasswordGeneratorTests
    {
        public class ShortestPasswordLength
        {
            [Fact]
            public void ReturnsShortestNounLength_ForSingleNounGenerator()
            {
                var adjectiveGenerator = Substitute.For<IRandomWordGenerator>();
                adjectiveGenerator.WordLengths.Returns(Array.Empty<int>());

                var nounGenerator = Substitute.For<IRandomWordGenerator>();
                nounGenerator.WordLengths.Returns(new[] { 1 });

                var passwordGenerator = new PasswordGenerator(adjectiveGenerator, nounGenerator);

                passwordGenerator.ShortestPasswordLength.Should().Be(1); 
            }

            [Fact]
            public void ReturnsShortestNounLength_ForMultipleGenerators()
            {
                var adjectiveGenerator = Substitute.For<IRandomWordGenerator>();
                adjectiveGenerator.WordLengths.Returns(new[] { 1, 3, 4, 5 });

                var nounGenerator = Substitute.For<IRandomWordGenerator>();
                nounGenerator.WordLengths.Returns(new[] { 1, 3, 5, 60 });

                var passwordGenerator = new PasswordGenerator(adjectiveGenerator, nounGenerator);

                passwordGenerator.ShortestPasswordLength.Should().Be(1);
            }
        }

        public class LongestPasswordLength
        {
            [Fact]
            public void ReturnsLongestNounLength_ForSingleNounGenerator()
            {
                var adjectiveGenerator = Substitute.For<IRandomWordGenerator>();
                adjectiveGenerator.WordLengths.Returns(Array.Empty<int>());

                var nounGenerator = Substitute.For<IRandomWordGenerator>();
                nounGenerator.WordLengths.Returns(new[] { 10 });

                var passwordGenerator = new PasswordGenerator(adjectiveGenerator, nounGenerator);

                passwordGenerator.LongestPasswordLength.Should().Be(10);
            }

            [Fact]
            public void ReturnsLongestNounLength_ForMultipleGenerators()
            {
                var adjectiveGenerator = Substitute.For<IRandomWordGenerator>();
                adjectiveGenerator.WordLengths.Returns(new[] { 1, 3, 4, 5 });

                var nounGenerator = Substitute.For<IRandomWordGenerator>();
                nounGenerator.WordLengths.Returns(new[] { 1, 3, 5, 60 });

                var passwordGenerator = new PasswordGenerator(adjectiveGenerator, nounGenerator);

                passwordGenerator.LongestPasswordLength.Should().Be(65);
            }
        }

        public class CalculateAvailableLengths
        {
            [Fact]
            public void ForSingleNounGenerator_OneChainIsGenerated()
            {
                var adjectiveGenerator = Substitute.For<IRandomWordGenerator>();
                adjectiveGenerator.WordLengths.Returns(Array.Empty<int>());

                var nounGenerator = Substitute.For<IRandomWordGenerator>();
                nounGenerator.WordLengths.Returns(new[] { 1 });

                var availableLengths = PasswordGenerator.CalculateAvailableLengths(adjectiveGenerator, nounGenerator);

                availableLengths.Count.Should().Be(1);
                availableLengths[1].Count.Should().Be(1);
            }

            [Fact]
            public void ForNounGeneratorWithThreeLengths_ThreeChainsAreGenerated()
            {
                var adjectiveGenerator = Substitute.For<IRandomWordGenerator>();
                adjectiveGenerator.WordLengths.Returns(Array.Empty<int>());

                var nounGenerator = Substitute.For<IRandomWordGenerator>();
                nounGenerator.WordLengths.Returns(new[] { 1, 5, 15 });

                var availableLengths = PasswordGenerator.CalculateAvailableLengths(adjectiveGenerator, nounGenerator);

                availableLengths.Count.Should().Be(3);
                availableLengths[1].Count.Should().Be(1);
                availableLengths[5].Count.Should().Be(1);
                availableLengths[15].Count.Should().Be(1);
            }

            [Fact]
            public void ForSingleNounGeneratorAndSingleAdjectiveGenerator_TwoChainsAreGenerated()
            {
                var adjectiveGenerator = Substitute.For<IRandomWordGenerator>();
                adjectiveGenerator.WordLengths.Returns(new[] { 1 });

                var nounGenerator = Substitute.For<IRandomWordGenerator>();
                nounGenerator.WordLengths.Returns(new[] { 1 });

                var availableLengths = PasswordGenerator.CalculateAvailableLengths(adjectiveGenerator, nounGenerator);

                availableLengths.Count.Should().Be(2);
                availableLengths[1].Count.Should().Be(1);
                availableLengths[2].Count.Should().Be(1);
            }

            [Fact]
            public void ForNounGeneratorAndAdjectiveGenerator_CrossJoinChainsAreGenerated()
            {
                var adjectiveGenerator = Substitute.For<IRandomWordGenerator>();
                adjectiveGenerator.WordLengths.Returns(new[] { 1, 2, 3 });

                var nounGenerator = Substitute.For<IRandomWordGenerator>();
                nounGenerator.WordLengths.Returns(new[] { 1, 2, 3 });

                var availableLengths = PasswordGenerator.CalculateAvailableLengths(adjectiveGenerator, nounGenerator);

                availableLengths.Keys.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5, 6 });
                availableLengths[1].Count.Should().Be(1);
                availableLengths[2].Count.Should().Be(2);
                availableLengths[3].Count.Should().Be(3);
                availableLengths[4].Count.Should().Be(3);
                availableLengths[5].Count.Should().Be(2);
                availableLengths[6].Count.Should().Be(1);
            }
        }

        public class Generate
        {
            private readonly IRandomWordGenerator nounGenerator;
            private readonly IRandomWordGenerator adjectiveGenerator;

            public Generate()
            {
                this.adjectiveGenerator = Substitute.For<IRandomWordGenerator>();
                this.nounGenerator = Substitute.For<IRandomWordGenerator>();
            }

            [Theory]
            [InlineData(new[] { 1 }, new[] { 1 }, 0)]
            [InlineData(new[] { 1 }, new[] { 1 }, 3)]
            [InlineData(new[] { 1, 3, 5, 7 }, new[] { 2, 4, 6, 8 }, 1)]
            [InlineData(new[] { 1, 3, 5, 7 }, new[] { 2, 4, 6, 8 }, 16)]
            public void ThrowsException_WhenLengthNotSupported(int[] adjectiveLengths, int[] wordLengths, int requestedLength)
            {
                this.adjectiveGenerator.WordLengths.Returns(adjectiveLengths);
                this.nounGenerator.WordLengths.Returns(wordLengths);
                var passwordGenerator = new PasswordGenerator(this.adjectiveGenerator, this.nounGenerator);

                Action generate = () => passwordGenerator.Generate(requestedLength);

                generate.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void F()
            {
            }
        }
    }
}