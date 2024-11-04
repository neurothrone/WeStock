using Microsoft.EntityFrameworkCore;
using WeStock.DAL.EFCore.Entities;

namespace WeStock.DAL.EFCore.Data;

public class WeStockDbContext : DbContext
{
    public WeStockDbContext(DbContextOptions<WeStockDbContext> options) : base(options)
    {
    }

    public DbSet<CollectionEntity> Collections { get; set; }
    public DbSet<SectionEntity> Sections { get; set; }
    public DbSet<ItemEntity> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CollectionEntity>()
            .HasMany(c => c.Sections)
            .WithOne(s => s.Collection)
            .HasForeignKey(s => s.CollectionId);

        modelBuilder.Entity<SectionEntity>()
            .HasMany(s => s.Items)
            .WithMany(i => i.Sections);
    }
}