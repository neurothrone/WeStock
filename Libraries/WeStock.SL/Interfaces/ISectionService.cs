using WeStock.DTO.Section;

namespace WeStock.SL.Interfaces;

public interface ISectionService
{
    public Action? OnSectionCreated { get; set; }
    public Action? OnSectionDeleted { get; set; }
    
    Task<SectionDto> CreateSectionAsync(CreateSectionDto section);
    Task<List<SectionDto>> RetrieveSectionsAsync();
    Task<List<SectionDto>> RetrieveSectionsByCollectionIdAsync(int collectionId);
    Task<SectionDto?> RetrieveSectionByIdAsync(int sectionId);
    Task<bool> UpdateSectionAsync(SectionDto section);
    Task<bool> DeleteSectionByIdAsync(int sectionId);
    Task<int> RetrieveSectionsCountByCollectionIdAsync(int collectionId);
}