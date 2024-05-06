namespace Reflectionnaire.Api.DataAccess
{
    internal class ReflectionnaireEntity : TableEntityBase
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ReflectionnaireTypeId { get; set; } = default!;
    }
}
