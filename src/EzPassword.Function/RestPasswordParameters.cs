namespace EzPassword.Function
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;
    using EzPassword.Core;

    internal class RestPasswordParameters : IPasswordParameters
    {
        public Language.Name Language { get; set; } = Core.Language.Name.English;

        [JsonPropertyName("count")]
        public int PasswordCount { get; set; } = 1;

        [JsonPropertyName("length")]
        public int PasswordLength { get; set; } = 20;

        public IEnumerable<string> Transformations { get; set; } = Enumerable.Empty<string>();

        public bool JsonOutput { get; set; } = false;
    }
}