namespace WeStock.DAL.Shared.Interfaces;

public interface ISectionRepository
{
    Task<ISectionEntity> CreateSectionAsync(ISectionEntity section);
    Task<List<ISectionEntity>> RetrieveSectionsAsync();
    Task<List<ISectionEntity>> RetrieveSectionsByCollectionIdAsync(int collectionId);
    Task<ISectionEntity?> RetrieveSectionByIdAsync(int id);
    Task<bool> UpdateSectionAsync(ISectionEntity section);
    Task<bool> DeleteSectionByIdAsync(int id);
    Task<int> RetrieveSectionsCountByCollectionIdAsync(int collectionId);
}