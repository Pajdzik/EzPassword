namespace EzPassword.Core
{
    using System.Collections.Generic;

    public interface IPasswordParameters
    {
        int PasswordCount { get; }

        int PasswordLength { get; }

        IEnumerable<string> Transformations { get; }
    }
}