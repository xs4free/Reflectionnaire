namespace Reflectionnaire.Shared
{
    public class ReflectionnaireData
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public IEnumerable<Question> Questions { get; set; } = [];
    }
}
