namespace EzPassword.Core.UnitTests
{
    using System;
    using Config;
    using Wiki;
    using Xunit;

    public class NounGeneratorTests
    {
        public class Ctor
        {
            [Fact]
            public void ThrowsException_WhenEmptyEndpointPassed()
            {
                Action ctor = () => new WikiWordGenerator(null, this.categoryMembersGenerator = Languages.Polish.NounCategory);
                Assert.Throws<ArgumentNullException>(ctor);
            }

            [Fact]
            public async void Foo()
            {
                //var g = new WikiWordGenerator("https://pl.wiktionary.org/w/api.php");
                //g.Subscribe()
            }
        }

        [Fact]
        public void GetRandomNoun_()
        {
            
        }
    }
}