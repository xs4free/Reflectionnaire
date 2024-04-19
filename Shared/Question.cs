namespace Reflectionnaire.Shared
{
    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ReflectionnaireId { get; set; }
        public QuestionType QuestionType { get; set; }
    }

    public enum QuestionType
    {
        Dummy,
        Things,
        People,
        Place,
        Execution
    }
}
