using CosmicObserverAPI.DTOs.CosmicTag;

namespace CosmicObserverAPI.Interfaces;

public interface ICosmicTagService
{
    Task<IEnumerable<TagResponse>> GetAllTagsAsync();

    Task<TagResponse?> GetTagByIdAsync(int id);

    Task<TagResponse?> CreateTagAsync(CreateTag newTag);

    Task<TagResponse?> UpdateTagAsync(CreateTag newTag, int id);

    Task<bool> DeleteTagAsync(int id);
}
