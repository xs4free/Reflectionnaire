using Azure;
using Azure.Data.Tables;
using System.Linq.Expressions;

namespace Reflectionnaire.Api.DataAccess
{
    internal class TableClientProxy(TableClient _tableClient) : ITableClient
    {
        public virtual Pageable<T> Query<T>(
            Expression<Func<T, bool>> filter,
            int? maxPerPage = null,
            IEnumerable<string>? select = null,
            CancellationToken cancellationToken = default) where T : class, ITableEntity
        {
            return _tableClient.Query<T>(filter, maxPerPage, select, cancellationToken);
        }

        public async Task<Response> AddEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity
        {
            return await _tableClient.AddEntityAsync<T>(entity, cancellationToken);
        }
    }

    public interface ITableClient
    {
        Pageable<T> Query<T>(
            Expression<Func<T, bool>> filter,
            int? maxPerPage = null,
            IEnumerable<string>? select = null,
            CancellationToken cancellationToken = default) where T : class, ITableEntity;

        Task<Response> AddEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity;
    }
}
