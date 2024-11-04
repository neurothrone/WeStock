using WeStock.BLL.Shared.Interfaces;
using WeStock.DTO.Section;
using WeStock.SL.Interfaces;
using WeStock.SL.Utils;

namespace WeStock.SL.Services;

public class SectionService : ISectionService, IDisposable
{
    private readonly ISectionManager _manager;

    public SectionService(ISectionManager manager)
    {
        _manager = manager;
    }

    #region ISectionService

    public Action? OnSectionCreated { get; set; }
    public Action? OnSectionDeleted { get; set; }

    public async Task<SectionDto> CreateSectionAsync(CreateSectionDto section)
    {
        var newSection = await _manager.CreateSectionAsync(section.MapToCreateModel());
        OnSectionCreated?.Invoke();
        return newSection.MapToDto();
    }

    public async Task<List<SectionDto>> RetrieveSectionsAsync()
    {
        var sections = await _manager.RetrieveSectionsAsync();
        return sections
            .Select(section => new SectionDto(section.Id, section.Name, section.CollectionId))
            .ToList();
    }

    public async Task<List<SectionDto>> RetrieveSectionsByCollectionIdAsync(int collectionId)
    {
        var sections = await _manager.RetrieveSectionsByCollectionIdAsync(collectionId);
        return sections
            .Select(section => section.MapToDto())
            .ToList();
    }

    public async Task<SectionDto?> RetrieveSectionByIdAsync(int sectionId)
    {
        var section = await _manager.RetrieveSectionByIdAsync(sectionId);
        return section?.MapToDto();
    }

    public Task<bool> UpdateSectionAsync(SectionDto section)
    {
        return _manager.UpdateSectionAsync(section.MapToModel());
    }

    public async Task<bool> DeleteSectionByIdAsync(int sectionId)
    {
        var deleted = await _manager.DeleteSectionByIdAsync(sectionId);
        if (deleted)
            OnSectionDeleted?.Invoke();
        return deleted;
    }

    public Task<int> RetrieveSectionsCountByCollectionIdAsync(int collectionId)
    {
        return _manager.RetrieveSectionsCountByCollectionIdAsync(collectionId);
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        OnSectionCreated = null;
        OnSectionDeleted = null;
    }

    #endregion
}