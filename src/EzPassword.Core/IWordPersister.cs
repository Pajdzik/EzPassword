namespace EzPassword.Core
{
    using System;
    using WikiClientLibrary.Pages;

    public interface IWordPersister : IObserver<WikiPage>
    {
    }
}