using WeStock.BLL.Shared.Models;

namespace WeStock.BLL.Shared.Interfaces;

public interface ISectionManager
{
    Task<Section> CreateSectionAsync(Section section);
    Task<List<Section>> RetrieveSectionsAsync();
    Task<List<Section>> RetrieveSectionsByCollectionIdAsync(int collectionId);
    Task<Section?> RetrieveSectionByIdAsync(int sectionId);
    Task<bool> UpdateSectionAsync(Section section);
    Task<bool> DeleteSectionByIdAsync(int sectionId);
    Task<int> RetrieveSectionsCountByCollectionIdAsync(int collectionId);
}