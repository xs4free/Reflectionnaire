namespace Reflectionnaire.Shared
{
    public class ReflectionnaireUserAnswers
    {
        public required string ReflectionnaireId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<QuestionAnswer> QuestionAnswers { get; set; } = [];
    }
}
