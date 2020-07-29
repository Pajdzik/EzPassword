namespace EzPassword.Core
{
    using System.Collections.Generic;

    public interface IPasswordParameters
    {
        Language.Name Language { get; }

        int PasswordCount { get; }

        int PasswordLength { get; }

        IEnumerable<string> Transformations { get; }
    }
}