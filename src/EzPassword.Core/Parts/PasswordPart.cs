using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzPassword.Core.Parts
{
    internal abstract class PasswordPart
    {
        public PasswordPart(char[] content)
        {
            this.Content = content;
        }

        public char[] Content { get; private set; }

        public int Length => this.Content.Length;
    }
}
