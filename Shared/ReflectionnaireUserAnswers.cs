namespace Reflectionnaire.Shared
{
    public class ReflectionnaireUserAnswers
    {
        public string ReflectionnaireId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<QuestionAnswer> QuestionAnswers { get; set; }
    }
}
