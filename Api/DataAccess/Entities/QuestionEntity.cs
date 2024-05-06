namespace Reflectionnaire.Api.DataAccess.Entities
{
    internal class QuestionEntity : TableEntityBase
    {
        public string? DescriptionNL { get; set; }
        public string? DescriptionEN { get; set; }
        public int Category { get; set; }
    }
}
