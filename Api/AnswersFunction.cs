using Microsoft.Azure.Functions.Worker;
using Reflectionnaire.Api.DataAccess;
using Reflectionnaire.Api.DataAccess.Entities;
using Reflectionnaire.Api.Mappers;
using Reflectionnaire.Shared;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reflectionnaire.Api
{
    public class AnswersFunction(ITableClientFactory _factory)
    {
        [Function("UserAnswers")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, 
            [Microsoft.Azure.Functions.Worker.Http.FromBody] ReflectionnaireUserAnswers answers)
        {
            var reflectionnaireClient = await _factory.CreateAsync(TableNames.Reflectionnaires);
            var reflectionnaire = reflectionnaireClient.Query<ReflectionnaireEntity>(
                e => e.PartitionKey == answers.ReflectionnaireId && e.RowKey == answers.ReflectionnaireId).FirstOrDefault();

            if (reflectionnaire == null || reflectionnaire.EndDate <= DateTime.Now)
            {
                return new NotFoundObjectResult(null);
            }

            var questionsClient = await _factory.CreateAsync(TableNames.Answers);
            var entity = AnswersMapper.ReflectionnaireUserAnswersToAnswersEntity(answers);
            await questionsClient.AddEntityAsync(entity);

            return new OkObjectResult(entity);
        }

        [Function("AllUsersAnswers")]
        public async Task<IActionResult> AllUsersAnswers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request,
            [FromQuery] Guid reflectionnaireId)
        {
            var reflectionnaireClient = await _factory.CreateAsync(TableNames.Reflectionnaires);
            string reflectionnaireIdText = reflectionnaireId.ToString("D");
            var reflectionnaire = reflectionnaireClient.Query<ReflectionnaireEntity>(
                e => e.PartitionKey == reflectionnaireIdText && e.RowKey == reflectionnaireIdText).FirstOrDefault();

            if (reflectionnaire == null)
            {
                return new NotFoundObjectResult(null);
            }

            var questionsClient = await _factory.CreateAsync(TableNames.Questions);
            var questionToCategoryLookup = questionsClient
                .Query<QuestionEntity>(answer => answer.PartitionKey == reflectionnaire.ReflectionnaireTypeId)
                .ToDictionary(entity => Convert.ToInt32(entity.RowKey), entity => entity.Category);

            var answersClient = await _factory.CreateAsync(TableNames.Answers);
            var entities = answersClient.Query<AnswersEntity>(answer => answer.PartitionKey == reflectionnaireIdText);

            var numberOfUsers = entities.GroupBy(entity => entity.RowKey).Count();

            var categoryTotals = entities
                .SelectMany(AnswersMapper.AnswerEntityToQuestionAnswer)
                .Select(entity => new { Category = questionToCategoryLookup[entity.QuestionId], entity.Score })
                .GroupBy(entity => entity.Category)
                .Select(group => new CategoryTotal { TotalScore = group.Sum(g => g.Score) / (float)numberOfUsers, Category = (Category)group.Key })
                .ToArray();

            return new OkObjectResult(new ReflectionnaireAllUserAnswers
            {
                ReflectionnaireId = reflectionnaireIdText,
                Name = reflectionnaire.Name,
                Description = reflectionnaire.Description,
                NumberOfRespondents = numberOfUsers,
                CategoryTotals = categoryTotals
            });
        }
    }
}
