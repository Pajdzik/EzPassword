namespace EzPassword.Transformation
{
    using System;
    using System.Linq;
    using EzPassword.Core.Parts;

    public class LowerCaseTransformation : ChangeCaseTransformation
    {
        public override string Keyword { get; } = "lower";
        
        protected override PasswordPart ChangeCase(Word word)
        {
            return new Word(word.Content.Select(c => Char.ToLowerInvariant(c)).ToArray());
        }
    }
}