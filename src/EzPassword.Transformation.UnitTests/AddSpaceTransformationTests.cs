namespace EzPassword.Transformation.UnitTests
{
    using System.Linq;
    using EzPassword.Core;
    using EzPassword.Core.Parts;
    using EzPassword.Transformation;
    using FluentAssertions;
    using Xunit;

    public class AddSpaceTransformationTests : TransformationTests
    {
        public class Transform 
        {
            private AddSpaceTransformation addSpaceTransformation;

            public Transform()
            {
                this.addSpaceTransformation = new AddSpaceTransformation();
            }
            
            [Fact]
            public void WhenEmptyPasswordPassed_EmptyPasswordReturned()
            {
                var emptyPassword = new Password(Enumerable.Empty<PasswordPart>());
                var resultPassword = this.addSpaceTransformation.Transform(emptyPassword);

                resultPassword.PasswordParts.Should().BeEmpty();
            }

            [Fact]
            public void WhenOnePartPassed_NoSpaceAdded()
            {
                var parts = GetPasswordParts("abc");
                var password = new Password(parts);
                var resultPassword = this.addSpaceTransformation.Transform(password);

                resultPassword.PasswordParts.Should().ContainSingle(part => part.ToString().Equals("abc"));
            }

            [Fact]
            public void WhenTwoPartsPassed_OneSpaceAdded()
            {
                var parts = GetPasswordParts("abc", "def");
                var password = new Password(parts);
                var resultPassword = this.addSpaceTransformation.Transform(password);

                resultPassword.PasswordParts.Should().HaveCount(3);
                resultPassword.ToString().Should().Be("abc def");
            }

            [Fact]
            public void WhenOnlySymbolsPassed_NoSpaceAdded()
            {
                var parts = new[] { "abc", "#$!" }.Select(part => new Symbol(part));
                var password = new Password(parts);
                var resultPassword = this.addSpaceTransformation.Transform(password);

                resultPassword.PasswordParts.Should().HaveCount(2);
                resultPassword.ToString().Should().Be("abc#$!");
            }

            [Theory]
            [InlineData("abc def xyz", "abc", "def", "xyz")]
            [InlineData("Abc Def  Xyz", "Abc", "Def", " Xyz")]
            [InlineData("Abc $#! XYZ", "Abc", "$#!", "XYZ")]
            [InlineData("a b c d e f", "a", "b", "c", "d", "e", "f")]
            public void WhenMultiplePartsPassed_MultipleSpacesInjected(string expectedPassword, params string[] stringParts)
            {
                var parts = GetPasswordParts(stringParts);
                var password = new Password(parts);
                var resultPassword = this.addSpaceTransformation.Transform(password);

                resultPassword.PasswordParts.Should().HaveCount((stringParts.Length * 2) - 1);
                resultPassword.ToString().Should().Be(expectedPassword);
            }
        }
    }
}