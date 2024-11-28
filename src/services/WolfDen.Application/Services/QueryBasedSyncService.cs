using System.Data.SqlClient;
using WolfDen.Application.Models;

public class QueryBasedSyncService
{
    private readonly string _sourceConnectionString;
    private readonly string _destinationConnectionString;
    private readonly int _batchSize;
    private readonly int _commandTimeout;
    public QueryBasedSyncService(
        string sourceConnectionString,
        string destinationConnectionString,
        int batchSize = 10000,
        int commandTimeout = 600)
    {
        _sourceConnectionString = sourceConnectionString;
        _destinationConnectionString = destinationConnectionString;
        _batchSize = batchSize;
        _commandTimeout = commandTimeout;
    }

    public async Task SyncTablesAsync(Dictionary<string, TableSyncConfig> syncConfigs)
    {
        foreach (var configPair in syncConfigs)
        {
            try
            {
                await SyncTableAsync(configPair.Value);
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex);

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
        };

        await bulkCopy.WriteToServerAsync(reader);

        if (!string.IsNullOrEmpty(config.PostSyncQuery))
        {
            await ExecuteQueryAsync(destinationConnection, config.PostSyncQuery);
        }

    }

    private async Task TruncateTableAsync(SqlConnection connection, string tableName)
    {
        using var cmd = new SqlCommand($"TRUNCATE TABLE {tableName}", connection);
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task ExecuteQueryAsync(SqlConnection connection, string query)
    {
        using var cmd = new SqlCommand(query, connection);
        await cmd.ExecuteNonQueryAsync();
    }
}
