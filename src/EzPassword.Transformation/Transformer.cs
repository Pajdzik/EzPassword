namespace EzPassword.Transformation
{
    using System.Collections.Generic;
    using EzPassword.Core;

    public class Transformer : ITransformation
    {
        private ICollection<ITransformation> transformations;

        public Transformer(ICollection<ITransformation> transformations)
        {
            this.transformations = transformations;
        }

        public Password Transform(Password password)
        {
            foreach (ITransformation transformation in this.transformations)
            {
                password = transformation.Transform(password);
            }

            return password;
        }
    }
}