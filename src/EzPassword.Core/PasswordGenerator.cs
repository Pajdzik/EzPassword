namespace EzPassword.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PasswordGenerator
    {
        private static readonly Random Random = new Random();

        private readonly IRandomWordGenerator adjectiveGenerator;
        private readonly IRandomWordGenerator nounGenerator;
        private readonly IDictionary<int, IList<PasswordGeneratingChain>> operatorsByLength;

        public PasswordGenerator(IRandomWordGenerator adjectiveGenerator, IRandomWordGenerator nounGenerator)
        {
            this.adjectiveGenerator = adjectiveGenerator;
            this.nounGenerator = nounGenerator;
            this.operatorsByLength = CalculateAvailableLengths(adjectiveGenerator, nounGenerator);
        }

        public int ShortestPasswordLength => this.operatorsByLength.Keys.Min();

        public int LongestPasswordLength => this.operatorsByLength.Keys.Max();

        public IEnumerable<int> AvailableLengths => this.operatorsByLength.Keys;

        public static IDictionary<int, IList<PasswordGeneratingChain>> CalculateAvailableLengths(IRandomWordGenerator adjectiveGenerator, IRandomWordGenerator nounGenerator)
        {
            var operatorsByLength = new Dictionary<int, IList<PasswordGeneratingChain>>();

            AddNounOnlyGenerators(nounGenerator, operatorsByLength);
            AddNounWithAdjectiveGenerators(adjectiveGenerator, nounGenerator, operatorsByLength);

            Dictionary<int, IList<PasswordGeneratingChain>> readOnlyCollectionDict
                = operatorsByLength.Keys.ToDictionary(key => key, key => operatorsByLength[key]);

            return readOnlyCollectionDict;
        }

        public Password Generate(int length)
        {
            if (!this.operatorsByLength.ContainsKey(length))
            {
                throw new ArgumentOutOfRangeException($"Generator doesn't contain words with {length} length");
            }

            IList<PasswordGeneratingChain> operators = this.operatorsByLength[length];
            PasswordGeneratingChain passwordGeneratingChain = PickRandomGenerator(operators);
            Password password = passwordGeneratingChain.Generate();

            return password;
        }

        private static void AddNounOnlyGenerators(IRandomWordGenerator nounGenerator, Dictionary<int, IList<PasswordGeneratingChain>> operatorsByLength)
        {
            foreach (int length in nounGenerator.WordLengths)
            {
                operatorsByLength.Add(
                    length,
                    new List<PasswordGeneratingChain>
                    {
                        new PasswordGeneratingChain(() => nounGenerator.GetRandomWord(length)),
                    });
            }
        }

        private static void AddNounWithAdjectiveGenerators(IRandomWordGenerator adjectiveGenerator, IRandomWordGenerator nounGenerator, Dictionary<int, IList<PasswordGeneratingChain>> operatorsByLength)
        {
            foreach (int nounLength in nounGenerator.WordLengths)
            {
                foreach (int adjectiveLength in adjectiveGenerator.WordLengths)
                {
                    int passwordLength = adjectiveLength + nounLength;

                    var passwordGeneratingChain = new PasswordGeneratingChain(
                            () => adjectiveGenerator.GetRandomWord(adjectiveLength),
                            () => nounGenerator.GetRandomWord(nounLength));

                    if (operatorsByLength.ContainsKey(passwordLength))
                    {
                        operatorsByLength[passwordLength].Add(passwordGeneratingChain);
                    }
                    else
                    {
                        operatorsByLength[passwordLength] = new List<PasswordGeneratingChain> { passwordGeneratingChain };
                    }
                }
            }
        }

        private static PasswordGeneratingChain PickRandomGenerator(IList<PasswordGeneratingChain> operators)
        {
            int randomIndex = Random.Next(operators.Count);
            return operators[randomIndex];
        }
    }
}
