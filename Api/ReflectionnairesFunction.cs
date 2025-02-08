using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Reflectionnaire.Api.DataAccess;
using Reflectionnaire.Api.DataAccess.Entities;
using Reflectionnaire.Api.Mappers;

namespace Reflectionnaire.Api
{
    public class ReflectionnairesFunction(ITableClientFactory _factory)
    {
        [Function("Reflectionnaires")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request, [FromQuery] string reflectionnaireId)
        {
            var reflectionnaireClient = await _factory.CreateAsync(TableNames.Reflectionnaires);
            var reflectionnaire = reflectionnaireClient.Query<ReflectionnaireEntity>(e => e.PartitionKey == reflectionnaireId && e.RowKey == reflectionnaireId).FirstOrDefault();
            if (reflectionnaire == null || reflectionnaire.EndDate <= DateTime.Now)
            {
                return new NotFoundObjectResult(null);
            }

            var questionsClient = await _factory.CreateAsync(TableNames.Questions);
            var entities = questionsClient.Query<QuestionEntity>(e => e.PartitionKey == reflectionnaire.ReflectionnaireTypeId).ToList();
            
            var result = ReflectionnaireMapper.ReflectionnaireEntityToReflectionnaire(reflectionnaire);
            result.Questions = entities.Select(QuestionsMapper.QuestionEntityToQuestionNL).ToList();

            return new OkObjectResult(result);
        }
    }
}
