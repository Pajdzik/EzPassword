namespace EzPassword.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class PasswordGenerator
    {
        private readonly static Random Random = new Random();

        private readonly IRandomWordGenerator adjectiveGenerator;
        private readonly IRandomWordGenerator nounGenerator;
        private readonly IDictionary<int, IReadOnlyCollection<PasswordGeneratingChain>> operatorsByLength;

        public PasswordGenerator(IRandomWordGenerator adjectiveGenerator, IRandomWordGenerator nounGenerator)
        {
            this.adjectiveGenerator = adjectiveGenerator;
            this.nounGenerator = nounGenerator;
            this.operatorsByLength = CalculateAvailableLengths(adjectiveGenerator, nounGenerator);
        }

        public int ShortestPasswordLength => this.operatorsByLength.Keys.Min();

        public int LongestPasswordLength => this.operatorsByLength.Keys.Max();

        public IEnumerable<int> AvailableLengths => this.operatorsByLength.Keys;

        public Password Generate(int length)
        {
            if (!this.operatorsByLength.ContainsKey(length))
            {
                throw new ArgumentOutOfRangeException();
            }

            IEnumerable<PasswordGeneratingChain> operators = this.operatorsByLength[length];

            return null;
        }

        public static IDictionary<int, IReadOnlyCollection<PasswordGeneratingChain>> CalculateAvailableLengths(IRandomWordGenerator adjectiveGenerator, IRandomWordGenerator nounGenerator)
        {
            var operatorsByLength = new Dictionary<int, List<PasswordGeneratingChain>>();

            AddNounOnlyGenerators(nounGenerator, operatorsByLength);
            AddNounWithAdjectiveGenerators(adjectiveGenerator, nounGenerator, operatorsByLength);

            Dictionary<int, IReadOnlyCollection<PasswordGeneratingChain>> readOnlyCollectionDict
                = operatorsByLength.Keys.ToDictionary(key => key, key => (IReadOnlyCollection<PasswordGeneratingChain>)operatorsByLength[key].AsReadOnly());

            return readOnlyCollectionDict;
        }

        private static void AddNounOnlyGenerators(IRandomWordGenerator nounGenerator, Dictionary<int, List<PasswordGeneratingChain>> operatorsByLength)
        {
            foreach (int length in nounGenerator.WordLengths)
            {
                operatorsByLength.Add(length, new List<PasswordGeneratingChain> { new PasswordGeneratingChain(nounGenerator) });
            }
        }

        private static void AddNounWithAdjectiveGenerators(IRandomWordGenerator adjectiveGenerator, IRandomWordGenerator nounGenerator, Dictionary<int, List<PasswordGeneratingChain>> operatorsByLength)
        {
            foreach (int nounLength in nounGenerator.WordLengths)
            {
                foreach (int adjectiveLength in adjectiveGenerator.WordLengths)
                {
                    int passwordLength = adjectiveLength + nounLength;

                    if (operatorsByLength.ContainsKey(passwordLength))
                    {
                        operatorsByLength[passwordLength].Add(new PasswordGeneratingChain(adjectiveGenerator, nounGenerator));
                    }
                    else
                    {
                        operatorsByLength[passwordLength] = new List<PasswordGeneratingChain> { new PasswordGeneratingChain(adjectiveGenerator, nounGenerator) };
                    }
                }
            }
        }

        private static PasswordGeneratingChain PickRandomGenerator(IEnumerable<PasswordGeneratingChain> operators)
        {
            throw new NotImplementedException();
        }
    }
}
