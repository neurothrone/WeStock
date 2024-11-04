using Microsoft.EntityFrameworkCore;
using WeStock.DAL.EFCore.Entities;

namespace WeStock.DAL.EFCore.Data;

public static class SeedData
{
    public static async Task AddInitialDataAsync(WeStockDbContext context)
    {
        // Add sample data only if the database is empty.
        if (await context.Collections.AnyAsync() ||
            await context.Sections.AnyAsync() ||
            await context.Items.AnyAsync())
            return;

        var collection = new CollectionEntity { Name = "Sample Collection" };
        context.Collections.Add(collection);

        var section1 = new SectionEntity { Name = "Sample Section 1", Collection = collection };
        var section2 = new SectionEntity { Name = "Sample Section 2", Collection = collection };
        context.Sections.AddRange(section1, section2);

        var item1 = new ItemEntity { Name = "Sample Item 1", Quantity = 2 };
        var item2 = new ItemEntity { Name = "Sample Item 2", Quantity = 4 };
        var item3 = new ItemEntity { Name = "Sample Item 3", Quantity = 6 };
        context.Items.AddRange(item1, item2, item3);

        section1.Items.Add(item1);
        section1.Items.Add(item2);
        section1.Items.Add(item3);

        await context.SaveChangesAsync();
    }
}