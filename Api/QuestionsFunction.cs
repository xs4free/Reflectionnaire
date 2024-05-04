using System.Net;
using Azure;
using Azure.Data.Tables;
using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Reflectionnaire.Api.DataAccess;
using Reflectionnaire.Shared;

namespace Reflectionnaire.Api
{
    public class QuestionsFunction
    {
        private readonly ILogger<QuestionsFunction> _logger;
        private readonly TableClient _tableClient;

        public QuestionsFunction(ILogger<QuestionsFunction> logger)
        {
            _logger = logger;

            var tableStorageEndpoint = Environment.GetEnvironmentVariable("TableStorageEndpointUri", EnvironmentVariableTarget.Process);
            _tableClient = new TableClient(
                new Uri(tableStorageEndpoint),
                "Questions",
                new DefaultAzureCredential());
        }

        [Function("Questions")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // var randomNumber = new Random();
            //
            // var result = Enumerable.Range(1, 16).Select(index => new Question
            // {
            //     Id = randomNumber.Next(),
            //     Description = $"Question {index}?",
            //     Category = index < 3 ? Category.Dummy : index < 6 ? Category.Things : index < 9 ? Category.People : index < 12 ? Category.Place : Category.Execution
            // }).ToArray();

            // Create the table if it doesn't already exist to verify we've successfully authenticated.
            await _tableClient.CreateIfNotExistsAsync();

            var questions = _tableClient.QueryAsync<QuestionEntity>(ent => ent.PartitionKey == "VR-Binding");

            var response = req.CreateResponse(HttpStatusCode.OK);
            
            await response.WriteAsJsonAsync(questions.GetAsyncEnumerator());

            return response;
        }
    }
}
