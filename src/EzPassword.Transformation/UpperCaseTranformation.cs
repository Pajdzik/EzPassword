namespace EzPassword.Transformation
{
    using System;
    using System.Linq;
    using EzPassword.Core.Parts;

    public class UpperCaseTransformation : ChangeCaseTransformation
    {
        public override string Keyword { get; } = "upper";

        protected override PasswordPart ChangeCase(Word word)
        {
            return new Word(word.Content.Select(c => Char.ToUpperInvariant(c)).ToArray());
        }
    }
}