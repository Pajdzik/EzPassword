namespace EzPassword.Core.Parts
{
    public abstract class PasswordPart
    {
        public PasswordPart(char[] content)
        {
            this.Content = content;
        }

        public char[] Content { get; private set; }

        public int Length => this.Content.Length;
    }
}
