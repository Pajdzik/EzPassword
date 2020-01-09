namespace EzPassword.Core.Parts
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class PasswordPart
    {
        public PasswordPart(IList<char> content)
        {
            this.Content = content;
        }

        public PasswordPart(string content)
            : this(content.ToCharArray())
        {
        }

        public IList<char> Content { get; private set; }

        public int Length => this.Content.Count;

        public override string ToString()
        {
            return new string(this.Content.ToArray());
        }
    }
}
