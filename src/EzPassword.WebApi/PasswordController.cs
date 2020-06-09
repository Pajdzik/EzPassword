namespace EzPassword.WebApi.Controllers
{
    using EzPassword.Core;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly ILogger<PasswordController> _logger;

        public PasswordController(ILogger<PasswordController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get([FromQuery] PasswordParameters parameters)
        {
            PasswordGenerator generator = PasswordGeneratorFactory.Create(
                "/Users/pajdziu/Repos/wiki/pl/adjectives",
                "/Users/pajdziu/Repos/wiki/pl/nouns",
                "adjectives_(\\d+).txt",
                "nouns_(\\d+).txt");

            return generator.Generate(parameters.PasswordLength).ToString();
        }
    }
}
