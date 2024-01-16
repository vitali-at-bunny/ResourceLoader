using ResourceLoader.Models;

namespace ResourceLoader.Services;

public class StatsService : IStatsService
{
    private readonly ILogger<StatsService> _logger;
    private ulong _req_total;
    private ulong _bytes_total;
    
    public StatsService(ILogger<StatsService> logger)
    {
        _logger = logger;
    }
    public Stats GetLastStats()
    {
        return new Stats{
            Requests = _req_total,
            Bytes = _bytes_total
        };
    }

    public void LogRequest(uint byteCount)
    {
        _logger.LogInformation($"Transferred {byteCount}");
        
        Interlocked.Add(ref _req_total, 1);
        Interlocked.Add(ref _bytes_total, byteCount);
    }
}