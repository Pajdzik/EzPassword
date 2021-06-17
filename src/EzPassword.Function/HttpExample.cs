namespace EzPassword.Function
{
    using System.Net;
    using EzPassword.Core;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;
    
    public class HttpExample
    {
        private readonly PasswordGenerator passwordGenerator;

        public HttpExample(PasswordGenerator passwordGenerator)
        {
            this.passwordGenerator = passwordGenerator;
        }

        [Function("EzPassword")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("HttpExample");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            Password password = passwordGenerator.Generate(20);
            response.WriteString("PASSWORD: " + password.ToString());

            return response;
        }
    }
}
