namespace Reflectionnaire.Shared
{
    public class ReflectionnaireType
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int NumberOfQuestions { get; set; }
        public bool Active { get; set; }
    }
}