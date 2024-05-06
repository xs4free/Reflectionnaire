using Reflectionnaire.Api.DataAccess.Entities;
using Reflectionnaire.Shared;
using Riok.Mapperly.Abstractions;
using System.Text.Json;

namespace Reflectionnaire.Api.Mappers
{
    [Mapper] // https://mapperly.riok.app/
    internal partial class AnswersMapper
    {
        [MapProperty(nameof(ReflectionnaireAnswers.ReflectionnaireId), nameof(AnswersEntity.PartitionKey))]
        [MapProperty(nameof(ReflectionnaireAnswers.UserId), nameof(AnswersEntity.RowKey))]
        [MapProperty(nameof(ReflectionnaireAnswers.QuestionAnswers), nameof(AnswersEntity.Answers), Use = nameof(QuestionAnswerToString))]
        public static partial AnswersEntity ReflectionnaireAnswersToAnswersEntity(ReflectionnaireAnswers answers);


        private static string QuestionAnswerToString(IEnumerable<QuestionAnswer> questionAnswers) =>
            JsonSerializer.Serialize(questionAnswers);
    }
}
