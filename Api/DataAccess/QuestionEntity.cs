using Azure;
using Azure.Data.Tables;
using Reflectionnaire.Shared;

namespace Reflectionnaire.Api.DataAccess
{
    public class QuestionEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string? DescriptionNL { get; set; }
        public string? DescriptionEN { get; set; }
        public Category? Category { get; set; }
    }
}
