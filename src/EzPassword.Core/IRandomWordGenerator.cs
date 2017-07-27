namespace EzPassword.Core
{
    using System.Collections.Generic;

    public interface IRandomWordGenerator
    {
        int ShortestWordLength { get; }

        int LongestWordLength { get; }

        IEnumerable<int> WordLengths { get; }

        string GetRandomWord();

        string GetRandomWord(int wordLength);
    }
}
