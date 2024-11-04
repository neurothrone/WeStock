using WeStock.BLL.Shared.Interfaces;
using WeStock.BLL.Shared.Models;
using WeStock.BLL.Shared.Utils;
using WeStock.DAL.EFCore.Entities;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.BLL.EFCore.Managers;

public class SectionManager : ISectionManager
{
    private readonly ISectionRepository _repository;

    public SectionManager(ISectionRepository repository)
    {
        _repository = repository;
    }

    #region ISectionManager

    public async Task<Section> CreateSectionAsync(Section section)
    {
        var entity = await _repository.CreateSectionAsync(section.MapToCreateEntity<SectionEntity>());
        return entity.MapToModel();
    }

    public async Task<List<Section>> RetrieveSectionsAsync()
    {
        var entities = await _repository.RetrieveSectionsAsync();
        return entities
            .Select(entity => entity.MapToModel())
            .ToList();
    }

    public async Task<List<Section>> RetrieveSectionsByCollectionIdAsync(int collectionId)
    {
        var entities = await _repository.RetrieveSectionsByCollectionIdAsync(collectionId);
        return entities
            .Select(entity => entity.MapToModel())
            .ToList();
    }

    public async Task<Section?> RetrieveSectionByIdAsync(int sectionId)
    {
        var entity = await _repository.RetrieveSectionByIdAsync(sectionId);
        return entity?.MapToModel();
    }

    public Task<bool> UpdateSectionAsync(Section section)
    {
        return _repository.UpdateSectionAsync(section.MapToEntity<SectionEntity>());
    }

    public Task<bool> DeleteSectionByIdAsync(int sectionId)
    {
        return _repository.DeleteSectionByIdAsync(sectionId);
    }

    public Task<int> RetrieveSectionsCountByCollectionIdAsync(int collectionId)
    {
        return _repository.RetrieveSectionsCountByCollectionIdAsync(collectionId);
    }

    #endregion
}