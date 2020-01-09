namespace EzPassword.Transformation.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EzPassword.Core;
    using EzPassword.Core.Parts;
    using EzPassword.Transformation;
    using FluentAssertions;
    using Xunit;

    public class AddSpaceTransformationTests
    {
        private static Password Transform(Password password)
        {
            var transformation = new AddSpaceTransformation();
            return transformation.Transform(password);
        }

        private static IEnumerable<PasswordPart> GetPasswordParts(params string[] parts)
        {
            return parts.Select(part => new Word(part));
        }

        [Fact]
        public void Tranform_WhenEmptyPasswordPassed_EmptyPasswordReturned()
        {
            var emptyPassword = new Password(Enumerable.Empty<PasswordPart>());
            var resultPassword = Transform(emptyPassword);

            resultPassword.PasswordParts.Should().BeEmpty();
        }

        [Fact]
        public void Tranform_WhenOnePartPassed_NoSpaceAdded()
        {
            var parts = GetPasswordParts("abc");
            var password = new Password(parts);
            var resultPassword = Transform(password);

            resultPassword.PasswordParts.Should().ContainSingle(part => part.ToString().Equals("abc"));
        }

        [Fact]
        public void Tranform_WhenTwoPartsPassed_OneSpaceAdded()
        {
            var parts = GetPasswordParts("abc", "def");
            var password = new Password(parts);
            var resultPassword = Transform(password);

            resultPassword.PasswordParts.Should().HaveCount(3);
            resultPassword.ToString().Should().Be("abc def");
        }

        [Theory]
        [InlineData("abc def xyz", "abc", "def", "xyz")]
        [InlineData("Abc Def  Xyz", "Abc", "Def", " Xyz")]
        [InlineData("Abc $#! XYZ", "Abc", "$#!", "XYZ")]
        [InlineData("a b c d e f", "a", "b", "c", "d", "e", "f")]
        public void Tranform_WhenMultiplePartsPassed_MultipleSpacesInjected(string expectedPassword, params string[] stringParts)
        {
            var parts = GetPasswordParts(stringParts);
            var password = new Password(parts);
            var resultPassword = Transform(password);

            resultPassword.PasswordParts.Should().HaveCount((stringParts.Length * 2) - 1);
            resultPassword.ToString().Should().Be(expectedPassword);
        }
    }
}
