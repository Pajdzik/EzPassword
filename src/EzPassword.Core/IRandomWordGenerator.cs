namespace EzPassword.Core
{
    internal interface IRandomWordGenerator
    {
        string GetRandomWord();

        string GetRandomWord(int wordLength);
    }
}