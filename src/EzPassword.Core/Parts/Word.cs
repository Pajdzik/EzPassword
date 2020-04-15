namespace EzPassword.Core.Parts
{
    public class Word : PasswordPart
    {
        public Word(char[] content)
            : base(content)
        {
        }

        public Word(string content)
            : base(content)
        {
        }
    }
}
