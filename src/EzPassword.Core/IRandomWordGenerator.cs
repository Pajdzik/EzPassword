namespace EzPassword.Core
{
    public interface IRandomWordGenerator
    {
        int ShortestWordLength { get; }

        int LongestWordLength { get; }

        string GetRandomWord();

        string GetRandomWord(int wordLength);
    }
}