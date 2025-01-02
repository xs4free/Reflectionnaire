using Reflectionnaire.Api.DataAccess.Entities;
using Reflectionnaire.Shared;

namespace Reflectionnaire.Api.Mappers
{
    internal static class QuestionsMapper
    {
        public static Question QuestionEntityToQuestionNL(QuestionEntity entity)
        {
            return new Question
            {
                Id = Convert.ToInt32(entity.RowKey),
                Description = entity.DescriptionNL ?? string.Empty,
                Category = (Category)entity.Category
            };
        }

        public static Question QuestionEntityToQuestionEN(QuestionEntity entity)
        {
            return new Question
            {
                Id = Convert.ToInt32(entity.RowKey),
                Description = entity.DescriptionEN ?? string.Empty,
                Category = (Category)entity.Category
            };
        }
    }
}
