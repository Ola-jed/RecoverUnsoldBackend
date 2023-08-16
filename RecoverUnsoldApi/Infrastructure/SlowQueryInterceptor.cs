using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Sentry;

namespace RecoverUnsoldApi.Infrastructure;

public class SlowQueryInterceptor : DbCommandInterceptor
{
    private const int SlowQueryThreshold = 300; // Milliseconds

    public override DbDataReader ReaderExecuted(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result)
    {
        var duration = eventData.Duration.TotalMilliseconds;
        if (duration > SlowQueryThreshold)
        {
            SentrySdk.CaptureMessage(
                $"Slow query detected : ({duration} ms) for SQL [{command.CommandText}]",
                SentryLevel.Warning
            );
        }

        return base.ReaderExecuted(command, eventData, result);
    }
}