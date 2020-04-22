namespace EzPassword.Transformation
{
    using System;
    using EzPassword.Core.Parts;

    public class UpperCaseTransformation : ChangeCaseTransformation
    {
        protected override PasswordPart ChangeCase(Word word)
        {
            word.Content[0] = Char.ToUpperInvariant(word.Content[0]);
            return word;
        }
    }
}