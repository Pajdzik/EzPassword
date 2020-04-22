namespace EzPassword.Transformation.UnitTests
{
    using System;
    using System.Linq;
    using EzPassword.Core;
    using EzPassword.Core.Parts;
    using FluentAssertions;
    using Xunit;

    public class UpperCaseTransformationTests
    {
        public class Transform : TransformationTests
        {
            private UpperCaseTransformation upperCaseTransformation;

            public Transform()
            {
                this.upperCaseTransformation = new UpperCaseTransformation();
            }

            [Fact]
            public void WhenEmptyPasswordPassed_EmptyPasswordReturned()
            {
                var emptyPassword = new Password(Enumerable.Empty<PasswordPart>());
                var resultPassword = this.upperCaseTransformation.Transform(emptyPassword);

                resultPassword.PasswordParts.Should().BeEmpty();
            }

            [Fact]
            public void WhenOnePartPassed_FirstLetterIsChangedToUpperCase()
            {
                var parts = GetPasswordParts("abc");
                var password = new Password(parts);
                var resultPassword = this.upperCaseTransformation.Transform(password);

                resultPassword.PasswordParts.Should().ContainSingle(part => part.ToString().Equals("Abc"));
            }

            [Theory]
            [InlineData("AbcDefXyz", "abc", "def", "xyz")]
            [InlineData("AbcDefXyz", "Abc", "Def", "Xyz")]
            [InlineData("Abc$#!XYZ", "Abc", "$#!", "XYZ")]
            [InlineData("ABCDEF", "a", "b", "c", "d", "e", "f")]
            public void WhenMultiplePartsPassed_EveryWordIsCapitalized(string expectedPassword, params string[] stringParts)
            {
                var parts = GetPasswordParts(stringParts);
                var password = new Password(parts);
                var resultPassword = this.upperCaseTransformation.Transform(password);

                resultPassword.PasswordParts.Should().HaveCount(stringParts.Length);
                resultPassword.ToString().Should().Be(expectedPassword);
            }
        }
    }
}