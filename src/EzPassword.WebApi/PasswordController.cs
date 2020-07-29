namespace EzPassword.WebApi.Controllers
{
    using System.IO;
    using EzPassword.Core;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IConfiguration configuration;

        private readonly ILogger<PasswordController> logger;

        private readonly string wordsDirectory;

        public PasswordController(IConfiguration configuration, ILogger<PasswordController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.wordsDirectory = configuration["AbsoluteWordsDirectory"];
        }

        [HttpGet]
        public string Get([FromQuery] PasswordParameters parameters)
        {
            this.logger.LogInformation($"Incoming request with parameters: {parameters}");

            string languageSymbol = Language.Get(parameters.Language);
            PasswordGenerator generator = PasswordGeneratorFactory.Create(
                Path.Combine(this.wordsDirectory, languageSymbol, "adjectives"),
                Path.Combine(this.wordsDirectory, languageSymbol, "nouns"),
                "adjectives_(\\d+).txt",
                "nouns_(\\d+).txt");

            return generator.Generate(parameters.PasswordLength).ToString();
        }
    }
}
