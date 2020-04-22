namespace EzPassword.Transformation.UnitTests
{
    using System.Linq;
    using EzPassword.Core;
    using EzPassword.Core.Parts;
    using FluentAssertions;
    using Xunit;

    public class LowerCaseTransformationTests
    {
        public class Transform : TransformationTests
        {
            private LowerCaseTransformation lowerCaseTransformation;

            public Transform()
            {
                this.lowerCaseTransformation = new LowerCaseTransformation();
            }

            [Fact]
            public void WhenEmptyPasswordPassed_EmptyPasswordReturned()
            {
                var emptyPassword = new Password(Enumerable.Empty<PasswordPart>());
                var resultPassword = this.lowerCaseTransformation.Transform(emptyPassword);

                resultPassword.PasswordParts.Should().BeEmpty();
            }

            [Fact]
            public void WhenOnePartPassed_FirstLetterIsChangedToUpperCase()
            {
                var parts = GetPasswordParts("Abc");
                var password = new Password(parts);
                var resultPassword = this.lowerCaseTransformation.Transform(password);

                resultPassword.PasswordParts.Should().ContainSingle(part => part.ToString().Equals("abc"));
            }

            [Theory]
            [InlineData("abcdefxyz", "abc", "def", "xyz")]
            [InlineData("abcdefxyz", "Abc", "Def", "Xyz")]
            [InlineData("abc$#!xyz", "aBC", "$#!", "xYz")]
            [InlineData("abcdef", "A", "B", "C", "D", "E", "F")]
            public void WhenMultiplePartsPassed_EveryWordIsCapitalized(string expectedPassword, params string[] stringParts)
            {
                var parts = GetPasswordParts(stringParts);
                var password = new Password(parts);
                var resultPassword = this.lowerCaseTransformation.Transform(password);

                resultPassword.PasswordParts.Should().HaveCount(stringParts.Length);
                resultPassword.ToString().Should().Be(expectedPassword);
            }
        }
    }
}