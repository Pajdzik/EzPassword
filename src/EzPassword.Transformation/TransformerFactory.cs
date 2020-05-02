namespace EzPassword.Transformation
{
    using System.Collections.Generic;
    using EzPassword.Core;

    public class TransformerFactory
    {
        public static Transformer CreateFromKeywords(IEnumerable<string> keywords)
        {
            var transformations = new List<ITransformation>();

            return new Transformer(transformations);
        }
    }
}