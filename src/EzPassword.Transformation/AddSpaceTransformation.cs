namespace EzPassword.Transformation
{
    using EzPassword.Core.Parts;

    public class AddSpaceTransformation : AddCharactersTransformation
    {
        protected override Symbol GetSymbol()
        {
            return new Symbol(" ");
        }
    }
}