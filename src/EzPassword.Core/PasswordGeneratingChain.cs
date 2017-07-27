namespace EzPassword.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EzPassword.Core.Parts;

    public class PasswordGeneratingChain
    {
        private Func<string>[] randomWordGenerators;

        public PasswordGeneratingChain(params Func<string>[] randomWordGenerators)
        {
            this.randomWordGenerators = randomWordGenerators;
        }

        public Password Generate()
        {
            IEnumerable<string> words = this.GenerateParts();
            IEnumerable<Word> parts = words
                .Select(word => word.ToArray())
                .Select(characters => new Word(characters));

            return new Password(parts);
        }

        private IEnumerable<string> GenerateParts()
        {
            foreach (Func<string> generate in this.randomWordGenerators)
            {
                yield return generate();
            }
        }
    }
}
