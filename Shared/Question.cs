namespace Reflectionnaire.Shared
{
    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        
        internal int ReflectionaireTypeId { get; set; }
        internal Language Language { get; set; }
    }
}
