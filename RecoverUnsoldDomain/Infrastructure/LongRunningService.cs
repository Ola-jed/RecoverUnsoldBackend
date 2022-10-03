namespace RecoverUnsoldDomain.Infrastructure;

public class LongRunningService : BackgroundService
{
    private readonly BackgroundWorkerQueue _queue;
    private readonly ILogger<LongRunningService> _logger;

    public LongRunningService(BackgroundWorkerQueue queue, ILogger<LongRunningService> logger)
    {
        _queue = queue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _queue.DequeueAsync(stoppingToken);
            _logger.LogInformation("Starting work item {WorkItem}", workItem.GetType().Name);
            await workItem(stoppingToken);
        }
    }
}