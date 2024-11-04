using Microsoft.EntityFrameworkCore;
using WeStock.DAL.EFCore.Data;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.EFCore.Repositories;

public class CollectionRepository : ICollectionRepository
{
    private readonly IDbContextFactory<WeStockDbContext> _contextFactory;

    public CollectionRepository(IDbContextFactory<WeStockDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    #region ICollectionRepository

    public async Task<ICollectionEntity> CreateCollectionAsync(ICollectionEntity collection)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        await context.AddAsync(collection);
        await context.SaveChangesAsync();
        return collection;
    }

    public async Task<List<ICollectionEntity>> RetrieveCollectionsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var collections = await context.Collections
            .AsNoTracking()
            .ToListAsync();
        return [..collections];
    }

    public async Task<ICollectionEntity?> RetrieveCollectionByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Collections
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> UpdateCollectionAsync(ICollectionEntity collection)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var collectionToUpdate = await context.Collections
            .FirstOrDefaultAsync(c => c.Id == collection.Id);
        if (collectionToUpdate is null)
            return false;

        collectionToUpdate.Name = collection.Name;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCollectionByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var deletedRows = await context.Collections.Where(c => c.Id == id).ExecuteDeleteAsync();
        return deletedRows > 0;
    }

    public async Task<int> GetItemCountInCollectionAsync(int collectionId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        int itemCount = await context.Sections
            // Filter out sections that are not related to the Collection.
            .Where(section => section.CollectionId == collectionId)
            // Get all the items in the sections that are related to the Collection.
            .SelectMany(section => section.Items)
            // Count only unique item:s since an item can be related to multiple sections.
            .Distinct()
            .CountAsync();

        return itemCount;
    }

    #endregion
}