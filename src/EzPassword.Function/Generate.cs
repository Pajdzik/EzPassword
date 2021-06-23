namespace EzPassword.Function
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using EzPassword.Core;
    using EzPassword.Transformation;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;

    public class Generate
    {
        private readonly PasswordGenerator passwordGenerator;

        public Generate(PasswordGenerator passwordGenerator)
        {
            this.passwordGenerator = passwordGenerator;
        }

        [Function("Generate")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(Generate));
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var parameters = await this.ParseParameters(req);
            string output = this.GetOutput(parameters);
            response.WriteString(output);

            return response;
        }

        private async Task<RestPasswordParameters> ParseParameters(HttpRequestData request)
        {
            if (request.Method == HttpMethod.Get.Method)
            {
                return new RestPasswordParameters();
            }

            string requestBody;
            using (StreamReader streamReader = new StreamReader(request.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }

            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            var parameters = JsonSerializer.Deserialize<RestPasswordParameters>(requestBody, options);

            return parameters;
        }

        private string GetOutput(RestPasswordParameters parameters)
        {
            var passwords = new List<Password>();
            Transformer transformer = TransformerFactory.CreateFromKeywords(parameters.Transformations);

            for (int i = 0; i < parameters.PasswordCount; i++)
            {
                Password password = passwordGenerator.Generate(parameters.PasswordLength);
                Password transformedPassword = transformer.Transform(password);
                passwords.Add(transformedPassword);
            }

            return this.GenerateOutput(passwords, parameters.JsonOutput);
        }

        private string GenerateOutput(IEnumerable<Password> passwords, bool generateJson)
        {
            IEnumerable<string> verbatimPasswords = passwords.Select(p => p.ToString());
            return generateJson
                    ? this.GenerateJsonOutput(verbatimPasswords)
                    : this.GenerateTextOutput(verbatimPasswords);
        }

        private string GenerateJsonOutput(IEnumerable<string> passwords)
        {
            if (passwords.Count() == 1)
            {
                return JsonSerializer.Serialize(passwords.First());
            }

            return JsonSerializer.Serialize(passwords);
        }

        private string GenerateTextOutput(IEnumerable<string> passwords)
        {
            return string.Join(Environment.NewLine, passwords);
        }
    }
}
