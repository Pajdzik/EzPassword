namespace EzPassword.Transformation
{
    using System;
    using EzPassword.Core.Parts;

    public class AddSpecialCharactersTransformation : AddCharactersTransformation
    {
        public override string Keyword { get; } = "special";

        private static Random Random = new Random();

        private static string[] SpecialCharacters = { 
            "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "+", "=",
            "{", "}", "[", "]", "|", "\\", ":", ";", "\"", "'", "<", ",", ">", ".", "?", "/" 
        };

        protected override Symbol GetSymbol()
        {
            string randomSymbol = GetRandomSymbol();
            return new Symbol(randomSymbol);
        }

        private static string GetRandomSymbol()
        {
            int index = Random.Next(SpecialCharacters.Length);
            return SpecialCharacters[index];
        }
    }
}