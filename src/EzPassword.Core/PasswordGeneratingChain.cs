namespace EzPassword.Core
{
    public class PasswordGeneratingChain
    {
        private IRandomWordGenerator[] randomWordGenerators;

        public PasswordGeneratingChain(params IRandomWordGenerator[] randomWordGenerators)
        {
            this.randomWordGenerators = randomWordGenerators;
        }
    }
}
