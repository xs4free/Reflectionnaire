namespace Reflectionnaire.Shared
{
    public class ReflectionnaireAllUserAnswers
    {
        public string ReflectionnaireId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int NumberOfRespondents { get; set; }
        public IEnumerable<CategoryTotal> CategoryTotals { get; set; } = [];
    }
}
