namespace EzPassword.Transformation
{
    using System.Collections.Generic;
    using System.Linq;
    using EzPassword.Core;
    using EzPassword.Core.Parts;

    public class L33tSpeakTransformation : ITransformation
    {
        private static IDictionary<char, char> L33tDictionary = new Dictionary<char, char> {
            { 'a', '4' },
            { 'b', '8' },
            { 'e', '3' },
            { 'i', '!' },
            { 'l', '1' },
            { 'o', '0' },
            { 's', '5' },
            { 't', '7' },
        };

        public Password Transform(Password password)
        {
            var transformedParts = 
                password.PasswordParts.Select(part => {
                    switch (part)
                    {
                        case Word word:
                            return this.Transform(word);
                        default:
                            return part;
                    }
                });

            return new Password(transformedParts);
        }

        private PasswordPart Transform(Word word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                char lowerCaseLetter = char.ToLowerInvariant(word.Content[i]);

                if (L33tDictionary.ContainsKey(lowerCaseLetter))
                {
                    word.Content[i] = L33tDictionary[lowerCaseLetter];
                }
            }

            return word;
        }
    }
}