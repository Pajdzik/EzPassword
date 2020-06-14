namespace EzPassword.Transformation
{
    using System.Collections.Generic;
    using EzPassword.Core;

    public class Transformer : ITransformation
    {
        public string Keyword { get; } = "transformer";

        private ICollection<ITransformation> transformations;

        public Transformer(ICollection<ITransformation> transformations)
        {
            this.transformations = transformations;
        }

        public Password Transform(Password password)
        {
            Password transformedPassport = new Password(password);

            foreach (ITransformation transformation in this.transformations)
            {
                transformedPassport = transformation.Transform(transformedPassport);
            }

            return transformedPassport;
        }
    }
}