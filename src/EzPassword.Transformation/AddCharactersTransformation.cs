namespace EzPassword.Transformation
{
    using System.Collections.Generic;
    using System.Linq;
    using EzPassword.Core;
    using EzPassword.Core.Parts;

    public abstract class AddCharactersTransformation : ITransformation
    {
        public abstract string Keyword { get; }

        protected abstract Symbol GetSymbol();

        public Password Transform(Password password)
        {
            var firstPart = password.PasswordParts.FirstOrDefault();
            if (firstPart == null)
            {
                return password;
            }

            var partsWithSpaces = new List<PasswordPart>(new[] { firstPart });

            foreach (PasswordPart part in password.PasswordParts.Skip(1))
            {
                if (part is Word)
                {
                    partsWithSpaces.Add(this.GetSymbol());
                }
                
                partsWithSpaces.Add(part);
            }

            return new Password(partsWithSpaces);
        }
    }
}