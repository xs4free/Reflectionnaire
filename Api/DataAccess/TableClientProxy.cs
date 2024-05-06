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

    }

    public interface ITableClient
    {
        Pageable<T> Query<T>(
            Expression<Func<T, bool>> filter,
            int? maxPerPage = null,
            IEnumerable<string>? select = null,
            CancellationToken cancellationToken = default) where T : class, ITableEntity;
    }
}
