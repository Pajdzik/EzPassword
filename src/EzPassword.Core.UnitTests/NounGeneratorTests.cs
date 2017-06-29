namespace EzPassword.Core.UnitTests
{
    using System;
    using Xunit;

    public class NounGeneratorTests
    {
        public class Ctor
        {
            [Fact]
            public void ThrowsException_WhenEmptyEndpointPassed()
            {
                Action ctor = () => new NounGenerator(null);
                Assert.Throws<ArgumentNullException>(ctor);
            }

            [Fact]
            public async void Foo()
            {
                var g = new NounGenerator("https://pl.wiktionary.org/w/api.php");
                var a = await g.GetRandomNoun();
            }
        }

        [Fact]
        public void GetRandomNoun_()
        {
            
        }
    }
}