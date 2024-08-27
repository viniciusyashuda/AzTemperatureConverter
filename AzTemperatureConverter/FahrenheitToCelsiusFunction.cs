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
    public class FahrenheitToCelsiusFunction
    {
        private readonly ILogger<FahrenheitToCelsiusFunction> _logger;

        public FahrenheitToCelsiusFunction(ILogger<FahrenheitToCelsiusFunction> log)
        {
            _logger = log;
        }

        [FunctionName("FahrenheitToCelsiusFunction")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Conversion" })]
        [OpenApiParameter(name: "fahrenheit", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The temperature in **Fahrenheit** to convert to Celsius")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The temparature in Celsius")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fahrenheitToCelsius/{fahrenheit}")] HttpRequest req, double fahrenheit)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request. Paramenter received (Temperature in Fahrenheit): " + fahrenheit);

            var celsius = Math.Round((fahrenheit - 32) * 5 / 9, 2);

            string responseMessage = $"{fahrenheit} Fahrenheit is equivalent to {celsius} Celsius";

            _logger.LogInformation($"Conversion perfomed. Result: {celsius} Celsius");

            return new OkObjectResult(responseMessage);
        }
    }
}

