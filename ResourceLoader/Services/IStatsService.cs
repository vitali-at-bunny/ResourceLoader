using ResourceLoader.Models;

namespace ResourceLoader.Services;

public interface IStatsService{
    void LogRequest(uint byteCount);
    Stats GetLastStats();
}