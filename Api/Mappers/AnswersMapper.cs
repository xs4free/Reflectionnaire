using Reflectionnaire.Api.DataAccess.Entities;
using Reflectionnaire.Shared;
using System.Text.Json;

namespace Reflectionnaire.Api.Mappers
{
    internal static class AnswersMapper
    {
        public static AnswersEntity ReflectionnaireUserAnswersToAnswersEntity(ReflectionnaireUserAnswers answers)
        {
            return new AnswersEntity
            {
                PartitionKey = answers.ReflectionnaireId,
                RowKey = answers.UserId.ToString(),
                Answers = JsonSerializer.Serialize(answers.QuestionAnswers)
            };
        }

        public static IEnumerable<QuestionAnswer> AnswerEntityToQuestionAnswer(AnswersEntity? entity)
        {
            if (entity == null)
            {
                return [];
            }

            return JsonSerializer.Deserialize<IEnumerable<QuestionAnswer>>(entity.Answers) ?? [];
        }
    }
}
