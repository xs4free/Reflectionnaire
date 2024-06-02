using Reflectionnaire.Api.DataAccess.Entities;
using Reflectionnaire.Shared;
using Riok.Mapperly.Abstractions;
using System.Text.Json;

namespace Reflectionnaire.Api.Mappers
{
    [Mapper] // https://mapperly.riok.app/
    internal partial class AnswersMapper
    {
        [MapProperty(nameof(ReflectionnaireUserAnswers.ReflectionnaireId), nameof(AnswersEntity.PartitionKey))]
        [MapProperty(nameof(ReflectionnaireUserAnswers.UserId), nameof(AnswersEntity.RowKey))]
        [MapProperty(nameof(ReflectionnaireUserAnswers.QuestionAnswers), nameof(AnswersEntity.Answers), Use = nameof(QuestionAnswerToString))]
        public static partial AnswersEntity ReflectionnaireUserAnswersToAnswersEntity(ReflectionnaireUserAnswers answers);


        private static string QuestionAnswerToString(IEnumerable<QuestionAnswer> questionAnswers) =>
            JsonSerializer.Serialize(questionAnswers);
    }
}
