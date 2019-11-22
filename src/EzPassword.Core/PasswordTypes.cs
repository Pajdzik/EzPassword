namespace EzPassword.Core
{
    using System;

    [Flags]
    public enum PasswordTypes
    {
        Unknown = 0,
        Words = 1 << 1,
        WordsSeparatedBySpecialCharacters = 1 << 2,
        LeftPadedBySpecialCharactes
    }
}
