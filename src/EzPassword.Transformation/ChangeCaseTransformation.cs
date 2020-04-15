namespace EzPassword.Transformation
{
    using System;
    using System.Linq;
    using EzPassword.Core;
    using EzPassword.Core.Parts;

    public abstract class ChangeCaseTransformation : ITransformation
    {
        public ChangeCaseTransformation()
        {
        }

        public Password Transform(Password password)
        {
            foreach (PasswordPart part in password.PasswordParts.Skip(1))
            {
                if (part is Word)
                {
                    this.ChangeCase((Word) part);
                }
            }

            return password;
        }

        protected abstract void ChangeCase(Word word);
    }
}