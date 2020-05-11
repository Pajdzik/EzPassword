namespace EzPassword.Core
{
    using System.Collections.Generic;

    public interface IPasswordParameters
    {
        int PasswordCount { get; }

        int PasswordLength { get; }

        string? WordsDirectory { get; }

        IEnumerable<string> Transformations { get; }
    }
}