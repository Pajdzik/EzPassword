namespace EzPassword.Transformation.UnitTests
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using EzPassword.Core;
    using EzPassword.Core.Parts;
    using EzPassword.Transformation;
    using FluentAssertions;
    using Xunit;

    public class L33tSpeakTransformationTests : TransformationTests
    {
        public class Transform 
        {
            private L33tSpeakTransformation l33tSpeakTransformation;

            public Transform()
            {
                this.l33tSpeakTransformation = new L33tSpeakTransformation();
            }
            
            [Fact]
            public void WhenEmptyPasswordPassed_EmptyPasswordReturned()
            {
                var emptyPassword = new Password(Enumerable.Empty<PasswordPart>());
                var resultPassword = this.l33tSpeakTransformation.Transform(emptyPassword);

                resultPassword.PasswordParts.Should().BeEmpty();
            }

            [Fact]
            public void WhenOnePartPassed_NoSpaceAdded()
            {
                var parts = GetPasswordParts("abc");
                var password = new Password(parts);
                var resultPassword = this.l33tSpeakTransformation.Transform(password);

                resultPassword.PasswordParts.Should().ContainSingle(part => part.ToString().Equals("48c"));
            }

             [Theory]
            [InlineData("48cd3fxy7", "abc", "def", "xyt")]
            [InlineData("48cD3fXY7", "Abc", "Def", "XYT")]
            [InlineData("480$#!483", "Abo", "$#!", "483")]
            [InlineData("483!1057", "A", "B", "E", "I", "L", "O", "S", "T")]
            [InlineData("483!1057", "a", "b", "e", "i", "l", "o", "s", "t")]
            public void WhenMultiplePartsPassed_EveryWordIsCapitalized(string expectedPassword, params string[] stringParts)
            {
                var parts = GetPasswordParts(stringParts);
                var password = new Password(parts);
                var resultPassword = this.l33tSpeakTransformation.Transform(password);

                resultPassword.PasswordParts.Should().HaveCount(stringParts.Length);
                resultPassword.ToString().Should().Be(expectedPassword);
            }
        }
    }
}
