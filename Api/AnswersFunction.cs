using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Reflectionnaire.Api.DataAccess;
using Reflectionnaire.Api.DataAccess.Entities;
using Reflectionnaire.Api.Mappers;
using Reflectionnaire.Shared;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Reflectionnaire.Api
{
    public class AnswersFunction(ITableClientFactory _factory)
    {
        [Function("UserAnswers")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, 
            [Microsoft.Azure.Functions.Worker.Http.FromBody] ReflectionnaireUserAnswers answers)
        {
            var reflectionnaireClient = await _factory.CreateAsync(TableNames.Reflectionnaires);
            var reflectionnaire = reflectionnaireClient.Query<ReflectionnaireEntity>(
                e => e.PartitionKey == answers.ReflectionnaireId && e.RowKey == answers.ReflectionnaireId).FirstOrDefault();

            if (reflectionnaire == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }

            var questionsClient = await _factory.CreateAsync(TableNames.Answers);
            var entity = AnswersMapper.ReflectionnaireUserAnswersToAnswersEntity(answers);
            await questionsClient.AddEntityAsync(entity);

            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
