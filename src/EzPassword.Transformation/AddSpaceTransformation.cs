namespace EzPassword.Transformation
{
    using System.Collections.Generic;
    using System.Linq;
    using EzPassword.Core;
    using EzPassword.Core.Parts;

    public class AddSpaceTransformation : ITransformation
    {
        public Password Transform(Password password)
        {
            var firstPart = password.PasswordParts.FirstOrDefault();
            if (firstPart == null)
            {
                return password;
            }

            var partsWithSpaces = new List<PasswordPart>(new [] { firstPart });

            foreach (PasswordPart part in password.PasswordParts.Skip(1))
            {
                partsWithSpaces.Add(new Symbol(" "));
                partsWithSpaces.Add(part);
            }

            return new Password(partsWithSpaces);
        }
    }
}