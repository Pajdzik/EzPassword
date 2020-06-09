namespace EzPassword.WebApi
{
    using System.Collections.Generic;
    using EzPassword.Core;
    
    public class PasswordParameters : IPasswordParameters
    {
        public int PasswordCount { get; set; }

        public int PasswordLength { get; set; }
    
        public IEnumerable<string> Transformations { get; }

        public override string ToString()
        {
            return $"{{ PasswordCount: {this.PasswordCount}; PasswordLength: {this.PasswordLength}; }}";
        }
    }
}