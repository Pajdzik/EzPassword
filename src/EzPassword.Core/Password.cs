namespace EzPassword.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using EzPassword.Core.Parts;

    [System.Diagnostics.DebuggerDisplay("Password: {this.ToString()}")]
    public class Password
    {
        public Password(IEnumerable<PasswordPart> passwordParts)
        {
            this.PasswordParts = passwordParts;
        }

        public Password(Password password)
            : this(password.PasswordParts.ToList())
        {
        }

        public IEnumerable<PasswordPart> PasswordParts { get; private set; }

        public override string ToString()
        {
            IEnumerable<string> parts = this.PasswordParts.Select(part => string.Join(string.Empty, part.Content));
            return string.Join(string.Empty, parts);
        }
    }
}
