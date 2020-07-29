namespace EzPassword.WebApi
{
    using System.Collections.Generic;
    using EzPassword.Core;
    
    public class PasswordParameters : IPasswordParameters
    {
        public Language.Name Language { get; set; } = Core.Language.Name.English;

        public int PasswordCount { get; set; } = 1;

        public int PasswordLength { get; set; } = 20;

        public IEnumerable<string> Transformations { get; }

        public override string ToString()
        {
            return $"{{ Language: {this.Language}; PasswordCount: {this.PasswordCount}; PasswordLength: {this.PasswordLength}; }}";
        }
    }
}