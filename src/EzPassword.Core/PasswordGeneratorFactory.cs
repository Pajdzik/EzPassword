namespace EzPassword.Core
{
    using Kpax.Abstraction.System.IO;

    public sealed class PasswordGeneratorFactory
    {
        public static PasswordGenerator Create(
            string adjectiveDirectoryPath,
            string nounDirectoryPath,
            string adjectiveFileNameRegex,
            string nounFileNameRegex)
        {
                    var adjectiveGenerator = new TextFileWordGenerator(
                        new DirectoryFacade(),
                        new FileFacade(),
                        adjectiveDirectoryPath,
                        adjectiveFileNameRegex);

                    var nounGenerator = new TextFileWordGenerator(
                        new DirectoryFacade(),
                        new FileFacade(),
                        nounDirectoryPath,
                        nounFileNameRegex);

                    return new PasswordGenerator(adjectiveGenerator, nounGenerator);
        }
    }
}
