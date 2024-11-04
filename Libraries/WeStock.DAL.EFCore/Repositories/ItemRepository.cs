using Microsoft.EntityFrameworkCore;
using WeStock.DAL.EFCore.Data;
using WeStock.DAL.EFCore.Entities;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.EFCore.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly IDbContextFactory<WeStockDbContext> _contextFactory;

    public ItemRepository(IDbContextFactory<WeStockDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    #region IItemRepository

    public async Task<IItemEntity> CreateItemAsync(IItemEntity item)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var itemEntity = new ItemEntity
        {
            Name = item.Name,
            Quantity = item.Quantity
        };
        if (item.SectionId != 0)
            await AddItemToSection(itemEntity, item.SectionId, context);

        await context.AddAsync(itemEntity);
        await context.SaveChangesAsync();

        return itemEntity;
    }

    public async Task<List<IItemEntity>> RetrieveItemsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var items = await context.Items
            .AsNoTracking()
            .ToListAsync();
        return [..items];
    }

    public async Task<List<IItemEntity>> RetrieveItemsBySectionIdAsync(int sectionId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var section = await context.Sections
            .AsNoTracking()
            .Include(sectionEntity => sectionEntity.Items)
            .FirstOrDefaultAsync(s => s.Id == sectionId);
        return section is null ? [] : [..section.Items];
    }

    public async Task<IItemEntity?> RetrieveItemByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<bool> UpdateItemAsync(IItemEntity item)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var itemToUpdate = await context.Items
            .Include(i => i.Sections) // Ensure sections are loaded
            .FirstOrDefaultAsync(i => i.Id == item.Id);

        if (itemToUpdate is null)
            return false;

        itemToUpdate.Name = item.Name;
        itemToUpdate.Quantity = item.Quantity;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteItemByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var deletedRows = await context.Items.Where(i => i.Id == id).ExecuteDeleteAsync();
        return deletedRows > 0;
    }

    public async Task<bool> AddItemToSection(IItemEntity item)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var fetchedItem = await context.Items
            .FirstOrDefaultAsync(i => i.Id == item.Id);
        if (fetchedItem is null)
            return false;

        if (!await AddItemToSection(fetchedItem, item.SectionId, context))
            return false;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveItemFromSection(IItemEntity item)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var fetchedItem = await context.Items
            .Include(i => i.Sections) // Ensure sections are loaded
            .FirstOrDefaultAsync(i => i.Id == item.Id);

        if (fetchedItem is null)
            return false;

        var section = fetchedItem.Sections.FirstOrDefault(s => s.Id == item.SectionId);
        if (section is null)
            return false;

        fetchedItem.Sections.Remove(section);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<IItemEntity>> SearchItemsByNameAsync(string? searchText = null)
    {
        // In this use case if no searchText is provided, return no results.
        if (string.IsNullOrWhiteSpace(searchText))
            return [];

        await using var context = await _contextFactory.CreateDbContextAsync();
        var items = await context.Items
            .AsNoTracking()
            // Source: https://learn.microsoft.com/en-us/ef/core/providers/sqlite/functions#string-functions
            .Where(item => EF.Functions.Like(item.Name, $"%{searchText}%"))
            .ToListAsync();
        return items;
    }

    public async Task<IEnumerable<IItemEntity>> SearchItemsByNameExcludingSectionAsync(
        int sectionId,
        string? searchText = null)
    {
        // In this use case if no searchText is provided, return all results that match the section id.
        if (string.IsNullOrWhiteSpace(searchText))
            searchText = string.Empty;

        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Items
            .AsNoTracking()
            .Where(item => EF.Functions.Like(item.Name, $"%{searchText}%"))
            .Where(item => item.Sections.All(section => section.Id != sectionId))
            .ToListAsync();
    }

    #endregion

    private async Task<bool> AddItemToSection(ItemEntity item, int sectionId, WeStockDbContext context)
    {
        var section = await context.Sections.FindAsync(sectionId);
        if (section is null)
            return false;

        item.Sections.Add(section);
        return true;
    }
}