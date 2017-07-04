namespace EzPassword.Core.Wiki
{
    using System;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Config;
    using WikiClientLibrary.Client;
    using WikiClientLibrary.Generators;

    public class WikiWordGenerator : IWordGenerator
    {
        private readonly CategoryMembersGenerator categoryMembersGenerator;

        public WikiWordGenerator(CategoryMembersGenerator categoryMembersGenerator)
        {
            this.categoryMembersGenerator = categoryMembersGenerator;
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            this.categoryMembersGenerator
                .EnumPagesAsync()
                .ForEachAsync(page => { observer.OnNext(page.Title); })
                .ContinueWith(task => { observer.OnCompleted(); });

            return Disposable.Empty;
        }
    }
}