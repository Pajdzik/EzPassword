namespace EzPassword.Transformation
{
    using EzPassword.Core;

    public interface ITransformation
    {
        Password Transform(Password password);
    }
}