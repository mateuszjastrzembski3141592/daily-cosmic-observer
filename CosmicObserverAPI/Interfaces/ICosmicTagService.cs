using CosmicObserverAPI.DTOs.Tags;

namespace CosmicObserverAPI.Interfaces;

public interface ICosmicTagService
{
    Task<IEnumerable<TagResponse>> GetAllTagsAsync();

    Task<TagResponse?> GetTagByIdAsync(int id);

    Task<TagResponse?> CreateTagAsync(CreateTag newTag);

    // TODO: UpdateTagAsync()

    Task<bool> DeleteTagAsync(int id);
}
