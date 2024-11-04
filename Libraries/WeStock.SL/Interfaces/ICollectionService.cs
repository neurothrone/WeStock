using WeStock.DTO.Collection;

namespace WeStock.SL.Interfaces;

public interface ICollectionService
{
    public Action? OnCollectionCreated { get; set; }
    public Action? OnCollectionDeleted { get; set; }

    Task<CollectionDto> CreateCollectionAsync(CreateCollectionDto collection);
    Task<List<CollectionDto>> RetrieveCollectionsAsync();
    Task<CollectionDto?> RetrieveCollectionByIdAsync(int collectionId);
    Task<bool> UpdateCollectionAsync(CollectionDto collection);
    Task<bool> DeleteCollectionByIdAsync(int collectionId);
    Task<int> GetItemCountInCollectionAsync(int collectionId);
}