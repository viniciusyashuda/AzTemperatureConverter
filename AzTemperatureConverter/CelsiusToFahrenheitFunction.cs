using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AzTemperatureConverter
{
    public class CelsiusToFahrenheitFunction
    {
        private readonly ILogger<CelsiusToFahrenheitFunction> _logger;

        public CelsiusToFahrenheitFunction(ILogger<CelsiusToFahrenheitFunction> log)
        {
            _logger = log;
        }

        [FunctionName("CelsiusToFahrenheitFunction")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Conversion" })]
        [OpenApiParameter(name: "celsius", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The temperature in **Celsius** to convert to Fahrenheit")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The temparature in Fahrenheit")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "CelsiusToFahrenheit/{celsius}")] HttpRequest req, double celsius)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request. Paramenter received (Temperature in Celsius): " + celsius);

            var fahrenheit = Math.Round((celsius * 9 / 5) + 32, 2);

            string responseMessage = $"{celsius} Celsius is equivalent to {fahrenheit} Fahrenheit";

            _logger.LogInformation($"Conversion perfomed. Result: {fahrenheit} Fahrenheit");

            return new OkObjectResult(responseMessage);
        }
    }
}

