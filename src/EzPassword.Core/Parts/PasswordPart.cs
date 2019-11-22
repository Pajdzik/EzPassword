namespace EzPassword.Core.Parts
{
    using System.Collections.Generic;

    public abstract class PasswordPart
    {
        public PasswordPart(IList<char> content)
        {
            this.Content = content;
        }

        public IList<char> Content { get; private set; }

        public int Length => this.Content.Count;
    }
}
