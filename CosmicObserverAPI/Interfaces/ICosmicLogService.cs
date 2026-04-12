using CosmicObserverAPI.DTOs.CosmicLog;
using CosmicObserverAPI.DTOs.CosmicTag;

namespace CosmicObserverAPI.Interfaces;

public interface ICosmicLogService
{
    Task<IEnumerable<LogResponse>> GetAllLogsAsync();

    Task<LogResponse> GetLogByIdAsync(int id);

    Task<IEnumerable<LogResponse>> GetLogsByCategoryAsync(Enum categories);

    Task<IEnumerable<LogResponse>> GetLogsByTagsAsync(string[] tags);

    Task<LogResponse> CreateLogAsync(CreateLog newLog);

    Task<LogResponse> UpdateLogAsync(CreateLog newLog, int id);

    Task<bool> DeleteLogAsync(int id);
}
