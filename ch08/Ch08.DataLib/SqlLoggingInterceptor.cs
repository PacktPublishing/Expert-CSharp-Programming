using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace Ch08.DataLib;

public class SqlLoggingInterceptor : DbCommandInterceptor
{
    private readonly SqlQueryLogger _logger;

    public SqlLoggingInterceptor(SqlQueryLogger logger)
    {
        _logger = logger;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        _logger.LogQuery(command.CommandText);
        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        _logger.LogQuery(command.CommandText);
        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }
}

public class SqlQueryLogger
{
    private readonly List<string> _queries = [];

    public void LogQuery(string sql)
    {
        _queries.Add(sql);
    }

    public string GetLastQuery()
    {
        return _queries.LastOrDefault() ?? "";
    }

    public void Clear()
    {
        _queries.Clear();
    }

    public IReadOnlyList<string> GetAllQueries()
    {
        return _queries.AsReadOnly();
    }
}