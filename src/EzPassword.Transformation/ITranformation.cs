namespace EzPassword.Transformation
{
    using EzPassword.Core;

    public interface ITransformation
    {
        string Keyword { get; }

        Password Transform(Password password);
    }
}