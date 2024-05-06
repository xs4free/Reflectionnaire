namespace Reflectionnaire.Api.DataAccess
{
    public interface ITableClientFactory
    {
        Task<ITableClient> CreateAsync(TableNames tableName);
    }
}