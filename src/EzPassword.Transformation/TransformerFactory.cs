namespace EzPassword.Transformation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TransformerFactory
    {
        private static Type[] availableTransformationTypes = {
            typeof(AddSpaceTransformation),
            typeof(AddSpecialCharactersTransformation),
            typeof(CamelCaseTransformation),
            typeof(L33tSpeakTransformation),
            typeof(LowerCaseTransformation),
            typeof(UpperCaseTransformation),
        };

        private static IDictionary<string, ITransformation> availableTransformations;

        static TransformerFactory()
        {
            availableTransformations = availableTransformationTypes
                .Select(type => (ITransformation) Activator.CreateInstance(type))
                .ToDictionary(transformation => transformation.Keyword);
        }

        public static Transformer CreateFromKeywords(IEnumerable<string> keywords)
        {
            var transformations = keywords.Select(keyword => availableTransformations[keyword]).ToList();
            return new Transformer(transformations);
        }
    }
}