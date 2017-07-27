namespace EzPassword.Core
{
    using System;

    [Flags]
    public enum PasswordType
    {
        Unknown = 0,
        Words = 1 << 1,
        WordsSeparatedBySpecialCharacters = 1 << 2,
        LeftPadedBySpecialCharactes
    }
}
