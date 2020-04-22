namespace EzPassword.Transformation.UnitTests
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using EzPassword.Core;
    using EzPassword.Core.Parts;
    using EzPassword.Transformation;
    using FluentAssertions;
    using Xunit;

    public class AddSpecialCharactersTransformationTests : TransformationTests
    {
        public class Transform 
        {
            private AddSpecialCharactersTransformation addSpecialCharactersTransformation;

            public Transform()
            {
                this.addSpecialCharactersTransformation = new AddSpecialCharactersTransformation();
            }
            
            [Fact]
            public void WhenEmptyPasswordPassed_EmptyPasswordReturned()
            {
                var emptyPassword = new Password(Enumerable.Empty<PasswordPart>());
                var resultPassword = this.addSpecialCharactersTransformation.Transform(emptyPassword);

                resultPassword.PasswordParts.Should().BeEmpty();
            }

            [Fact]
            public void WhenOnePartPassed_NoSpaceAdded()
            {
                var parts = GetPasswordParts("abc");
                var password = new Password(parts);
                var resultPassword = this.addSpecialCharactersTransformation.Transform(password);

                resultPassword.PasswordParts.Should().ContainSingle(part => part.ToString().Equals("abc"));
            }

            [Fact]
            public void WhenTwoPartsPassed_OneSpaceAdded()
            {
                var parts = GetPasswordParts("abc", "def");
                var password = new Password(parts);
                var resultPassword = this.addSpecialCharactersTransformation.Transform(password);

                resultPassword.PasswordParts.Should().HaveCount(3);
                Regex.IsMatch(resultPassword.ToString(), @"abc[\W_]def").Should().BeTrue();
            }

            [Fact]
            public void WhenOnlySymbolsPassed_NoSpaceAdded()
            {
                var parts = new[] { "abc", "#$!" }.Select(part => new Symbol(part));
                var password = new Password(parts);
                var resultPassword = this.addSpecialCharactersTransformation.Transform(password);

                resultPassword.PasswordParts.Should().HaveCount(2);
                resultPassword.ToString().Should().Be("abc#$!");
            }

            [Theory]
            [InlineData(@"abc[\W_]def[\W_]xyz", "abc", "def", "xyz")]
            [InlineData(@"Abc[\W_]Def[\W_] Xyz", "Abc", "Def", " Xyz")]
            [InlineData(@"Abc[\W_]\$\#\![\W_]XYZ", "Abc", "$#!", "XYZ")]
            [InlineData(@"a[\W_]b[\W_]c[\W_]d[\W_]e[\W_]f", "a", "b", "c", "d", "e", "f")]
            public void WhenMultiplePartsPassed_MultipleSpacesInjected(string expectedPattern, params string[] stringParts)
            {
                var parts = GetPasswordParts(stringParts);
                var password = new Password(parts);
                var resultPassword = this.addSpecialCharactersTransformation.Transform(password);

                System.Console.WriteLine("D1: "+ resultPassword.ToString());
                resultPassword.PasswordParts.Should().HaveCount((stringParts.Length * 2) - 1);
                Regex.IsMatch(resultPassword.ToString(), expectedPattern).Should().BeTrue();
            }
        }
    }
}