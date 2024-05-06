using Azure.Data.Tables;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Reflectionnaire.Api.Options;

namespace Reflectionnaire.Api.DataAccess
{
    internal class TableClientFactory : ITableClientFactory
    {
        private readonly ReflectionnaireOptions _options;

        public TableClientFactory(IOptions<ReflectionnaireOptions> options)
        {
            _options = options.Value;
        }

        public async Task<ITableClient> CreateAsync(TableNames tableName)
        {
            TableServiceClient? tableService = null;

            if (!string.IsNullOrEmpty(_options.TableStorageConnectionString))
            {
                tableService = new TableServiceClient(_options.TableStorageConnectionString);
            }
            else
            {
                if (!string.IsNullOrEmpty(_options.TableStorageEndpointUri))
                {
                    tableService = new TableServiceClient(
                        new Uri(_options.TableStorageEndpointUri),
                        new DefaultAzureCredential());
                }
            }

            if (tableService != null)
            {
                var client = tableService.GetTableClient(tableName.ToString());

                // Create the table if it doesn't already exist to verify we've successfully authenticated.
                await client.CreateIfNotExistsAsync();

                return new TableClientProxy(client);
            }

            throw new InvalidOperationException("Unable to create TableServiceClient");
        }
    }
}
