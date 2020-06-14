namespace EzPassword.Transformation
{
    using System.Collections.Generic;
    using System.Linq;
    using EzPassword.Core;
    using EzPassword.Core.Parts;

    public abstract class ChangeCaseTransformation : ITransformation
    {
        public abstract string Keyword { get; }

        public Password Transform(Password password)
        {
            IEnumerable<PasswordPart> parts =
                password.PasswordParts.Select(part => {
                    switch (part)
                    {
                        case Word word:
                            return this.ChangeCase(word);
                        default:
                            return part;
                    }
                });

            return new Password(parts);
        }

        protected abstract PasswordPart ChangeCase(Word word);
    }
}