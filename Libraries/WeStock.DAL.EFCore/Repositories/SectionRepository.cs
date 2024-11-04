using Microsoft.EntityFrameworkCore;
using WeStock.DAL.EFCore.Data;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.EFCore.Repositories;

public class SectionRepository : ISectionRepository
{
    private readonly IDbContextFactory<WeStockDbContext> _contextFactory;

    public SectionRepository(IDbContextFactory<WeStockDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    #region ISectionRepository

    public async Task<ISectionEntity> CreateSectionAsync(ISectionEntity section)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        await context.AddAsync(section);
        await context.SaveChangesAsync();
        return section;
    }

    public async Task<List<ISectionEntity>> RetrieveSectionsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var sections = await context.Sections
            .AsNoTracking()
            .ToListAsync();
        return [..sections];
    }

    public async Task<List<ISectionEntity>> RetrieveSectionsByCollectionIdAsync(int collectionId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var sections = await context.Sections
            .Where(section => section.CollectionId == collectionId)
            .AsNoTracking()
            .ToListAsync();
        return [..sections];
    }

    public async Task<ISectionEntity?> RetrieveSectionByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Sections
            .AsNoTracking()
            .FirstOrDefaultAsync(section => section.Id == id);
    }

    public async Task<bool> UpdateSectionAsync(ISectionEntity section)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var sectionToUpdate = await context.Sections
            .FirstOrDefaultAsync(s => s.Id == section.Id);
        if (sectionToUpdate is null)
            return false;

        sectionToUpdate.Name = section.Name;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSectionByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var deletedRows = await context.Sections.Where(s => s.Id == id).ExecuteDeleteAsync();
        return deletedRows > 0;
    }

    public async Task<int> RetrieveSectionsCountByCollectionIdAsync(int collectionId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return context.Sections.Count(section => section.CollectionId == collectionId);
    }

    #endregion
}