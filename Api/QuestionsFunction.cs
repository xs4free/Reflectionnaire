using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using ToiToiToi.Shared;

namespace ToiToiToi.Api
{
    public class QuestionsFunction
    {
        private readonly ILogger<QuestionsFunction> _logger;

        public QuestionsFunction(ILogger<QuestionsFunction> logger)
        {
            _logger = logger;
        }

        [Function("Questions")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var randomNumber = new Random();

            var result = Enumerable.Range(1, 16).Select(index => new Question
            {
                Id = randomNumber.Next(),
                Description = $"Question {index}?",
                QuestionType = (QuestionType)randomNumber.Next(0, (int)QuestionType.Execution)
            }).ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
