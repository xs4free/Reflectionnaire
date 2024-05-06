using Reflectionnaire.Api.DataAccess;
using Reflectionnaire.Shared;
using Riok.Mapperly.Abstractions;

namespace Reflectionnaire.Api.Mappers
{
    [Mapper] // https://mapperly.riok.app/
    internal partial class QuestionsMapper
    {
        [MapProperty(nameof(QuestionEntity.RowKey), nameof(Question.Id))]
        [MapProperty(nameof(QuestionEntity.DescriptionNL), nameof(Question.Description))]
        public static partial Question QuestionEntityToQuestionNL(QuestionEntity entity);

        [MapProperty(nameof(QuestionEntity.RowKey), nameof(Question.Id))]
        [MapProperty(nameof(QuestionEntity.DescriptionEN), nameof(Question.Description))]
        public static partial Question QuestionEntityToQuestionEN(QuestionEntity entity);

    }
}
