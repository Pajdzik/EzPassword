namespace EzPassword.Transformation
{
    using EzPassword.Core.Parts;

    public class AddSpaceTransformation : AddCharactersTransformation
    {
        public override string Keyword { get; } = "space";

        protected override Symbol GetSymbol()
        {
            return new Symbol(" ");
        }
    }
}