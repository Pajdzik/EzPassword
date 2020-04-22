namespace EzPassword.Transformation.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using EzPassword.Core;
    using EzPassword.Core.Parts;

    public abstract class TransformationTests
    {
        protected static IEnumerable<PasswordPart> GetPasswordParts(params string[] parts)
        {
            return parts.Select(part => new Word(part));
        }
    }
}