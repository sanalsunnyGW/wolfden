using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Common;
using System.Data.SqlClient;
using WolfDen.Application.Constants;
using WolfDen.Application.Models;

public class QueryBasedSyncService
{
    private readonly string _sourceConnectionString;
    private readonly string _destinationConnectionString;
    private readonly int _batchSize;
    private readonly int _commandTimeout;
    private readonly ILogger<QueryBasedSyncService> _logger;
    public QueryBasedSyncService(
        string sourceConnectionString,
        string destinationConnectionString,
        ILogger<QueryBasedSyncService> logger,
        int batchSize = 10000,
        int commandTimeout = 600)
    {
        _sourceConnectionString = sourceConnectionString;
        _destinationConnectionString = destinationConnectionString;
        _batchSize = batchSize;
        _commandTimeout = commandTimeout;
        _logger = logger;
    }

    public async Task SyncTablesAsync()
    {
        Dictionary<string, TableSyncConfig> configs = SyncConfigs.GetSyncConfigs();
        foreach (var configPair in configs)
        {
            try
            {
                _logger.LogInformation($"Starting sync for: {configPair.Key}");
                await SyncTableAsync(configPair.Value);
                _logger.LogInformation($"Completed sync for: {configPair.Key}");
            }
            catch (Exception ex )
            {
                _logger.LogError(ex, $"Error syncing {configPair.Key}");

            }
        }
    }

    private async Task SyncTableAsync(TableSyncConfig config)
    {
        using var sourceConnection = new SqlConnection(_sourceConnectionString);
        using var destinationConnection = new SqlConnection(_destinationConnectionString);

        await sourceConnection.OpenAsync();
        await destinationConnection.OpenAsync();


        if (!string.IsNullOrEmpty(config.PreSyncSrcQuery))
        {
            await ExecuteQueryAsync(sourceConnection, config.PreSyncSrcQuery);
        }

        if (!string.IsNullOrEmpty(config.PreSyncDestQuery))
        {
            await ExecuteQueryAsync(destinationConnection, config.PreSyncDestQuery);
        }

        if (config.TruncateDestination)
        {
            await TruncateTableAsync(destinationConnection, config.DestinationTable);
        }

        using var sourceCmd = new SqlCommand(config.SourceQuery, sourceConnection)
        {
            CommandTimeout = _commandTimeout
        };

        using var reader = await sourceCmd.ExecuteReaderAsync();
        using var bulkCopy = new SqlBulkCopy(destinationConnection)
        {
            DestinationTableName = config.DestinationTable,
            BatchSize = _batchSize,
            BulkCopyTimeout = _commandTimeout
        };

        if (config.ColumnMappings != null && config.ColumnMappings.Count > 0)
        {
            foreach (var mapping in config.ColumnMappings)
            {
                bulkCopy.ColumnMappings.Add(mapping.Key, mapping.Value);
            }
        }
        else
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                bulkCopy.ColumnMappings.Add(reader.GetName(i), reader.GetName(i));
            }
        }

        var totalRows = 0L;
        bulkCopy.NotifyAfter = _batchSize;
        bulkCopy.SqlRowsCopied += (sender, e) =>
        {
            totalRows = e.RowsCopied;
            _logger.LogInformation($"Copied {e.RowsCopied:N0} rows to {config.DestinationTable}");
        };

        await bulkCopy.WriteToServerAsync(reader);

        if (!string.IsNullOrEmpty(config.PostSyncQuery))
        {
            await ExecuteQueryAsync(destinationConnection, config.PostSyncQuery);
        }
        _logger.LogInformation($"Total rows copied to {config.DestinationTable}: {totalRows:N0}");

    }

    private async Task TruncateTableAsync(SqlConnection connection, string tableName)
    {
        _logger.LogInformation($"Truncating table: {tableName}");
        using var cmd = new SqlCommand($"TRUNCATE TABLE {tableName}", connection);
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task ExecuteQueryAsync(SqlConnection connection, string query)
    {
        using var cmd = new SqlCommand(query, connection);
        await cmd.ExecuteNonQueryAsync();
    }
}
