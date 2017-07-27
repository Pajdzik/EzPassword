using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzPassword.Core
{
    [Flags]
    public enum PasswordType
    {
        Unknown = 0,
        Words = 1 << 1,
        WordsSeparatedBySpecialCharacters = 1 << 2,
        LeftPadedBySpecialCharactes
    }
}
