namespace EzPassword.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MultiLangPasswordGenerator
    {
        private IDictionary<Language.Name, PasswordGenerator> generators;

        public MultiLangPasswordGenerator()
        {
            this.generators = new Dictionary<Language.Name, PasswordGenerator>();
        }

        public MultiLangPasswordGenerator((Language.Name lang, PasswordGenerator generator)[] generators)
            : this()
        {
            if (generators != null)
            {
                foreach ((Language.Name lang, PasswordGenerator generator) in generators)
                {
                    this.generators.Add(lang, generator);
                }
            }
        }

        public Password Generate(Language.Name language, int length)
        {
            if (!this.generators.ContainsKey(language))
            {
                throw new InvalidOperationException();
            }

            return this.generators[language].Generate(length);
        }
    }
}