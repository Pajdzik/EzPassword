namespace EzPassword.WebApi
{
    using System.Collections.Generic;
    using EzPassword.Core;
    
    public class PasswordParameters : IPasswordParameters
    {
        public int PasswordCount { get; }

        public int PasswordLength { get; }

        public string? WordsDirectory { get; }
    
        public IEnumerable<string> Transformations { get; }
    }
}