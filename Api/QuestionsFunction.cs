using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Reflectionnaire.Api.DataAccess;
using Reflectionnaire.Api.DataAccess.Entities;
using Reflectionnaire.Api.Mappers;
using Reflectionnaire.Shared;

namespace Reflectionnaire.Api
{
    public class QuestionsFunction(ITableClientFactory _factory)
    {
        [Function("Questions")]
        public async Task<IEnumerable<Question>> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request, [FromQuery] string reflectionnaireId)
        {
            var reflectionnaireClient = await _factory.CreateAsync(TableNames.Reflectionnaires);
            var reflectionnaire = reflectionnaireClient.Query<ReflectionnaireEntity>(e => e.PartitionKey == reflectionnaireId && e.RowKey == reflectionnaireId).FirstOrDefault();
            if (reflectionnaire == null)
            {
                return [];
            }

            var questionsClient = await _factory.CreateAsync(TableNames.Questions);
            var entities = questionsClient.Query<QuestionEntity>(e => e.PartitionKey == reflectionnaire.ReflectionnaireTypeId).ToList();
            
            return entities.Select(QuestionsMapper.QuestionEntityToQuestionNL).ToList();
        }
    }
}
