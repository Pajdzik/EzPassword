﻿namespace EzPassword.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EzPassword.Core.Parts;

    public class Password
    {
        public Password(IEnumerable<PasswordPart> passwordParts)
        {
            this.PasswordParts = passwordParts;
        }

        public IEnumerable<PasswordPart> PasswordParts { get; private set; }

        public override string ToString()
        {
            IEnumerable<string> parts = this.PasswordParts.Select(part => new String(part.Content));
            return String.Join(String.Empty, parts);
        }
    }
}