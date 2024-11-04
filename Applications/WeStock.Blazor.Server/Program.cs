using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using WeStock.Blazor.Server.Components;
using WeStock.Blazor.Server.State;
using WeStock.BLL.EFCore.Managers;
using WeStock.BLL.InMemory.Managers;
using WeStock.BLL.Shared.Interfaces;
using WeStock.DAL.EFCore.Data;
using WeStock.DAL.EFCore.Repositories;
using WeStock.DAL.InMemory.Data;
using WeStock.DAL.InMemory.Repositories;
using WeStock.DAL.Shared.Interfaces;
using WeStock.SL.Interfaces;
using WeStock.SL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<AppState>();

// SL
builder.Services.AddScoped<ICollectionService, CollectionService>(provider =>
{
    var collectionService = new CollectionService(provider.GetRequiredService<ICollectionManager>());
    var appState = provider.GetRequiredService<AppState>();

    collectionService.OnCollectionCreated += appState.IncrementCollectionCount;
    collectionService.OnCollectionDeleted += appState.DecrementCollectionCount;

    return collectionService;
});
builder.Services.AddScoped<ISectionService, SectionService>(provider =>
{
    var sectionService = new SectionService(provider.GetRequiredService<ISectionManager>());
    var appState = provider.GetRequiredService<AppState>();

    sectionService.OnSectionCreated += appState.IncrementSectionCount;
    sectionService.OnSectionDeleted += appState.DecrementSectionCount;

    return sectionService;
});
builder.Services.AddScoped<IItemService, ItemService>(provider =>
{
    var itemService = new ItemService(provider.GetRequiredService<IItemManager>());
    var appState = provider.GetRequiredService<AppState>();

    itemService.OnItemCreated += appState.IncrementItemCount;
    itemService.OnItemDeleted += appState.DecrementItemCount;

    return itemService;
});

WebApplication app;

if (builder.Environment.IsEnvironment("Prototyping"))
{
    // NOTE: This approach for some reason requires loading web assets manually.
    StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

    #region In-Memory

    // BLL
    builder.Services.AddScoped<ICollectionManager, InMemoryCollectionManager>();
    builder.Services.AddScoped<ISectionManager, InMemorySectionManager>();
    builder.Services.AddScoped<IItemManager, InMemoryItemManager>();

    // DAL
    builder.Services.AddSingleton<DataStore>(_ =>
    {
        var dataStore = new DataStore();
        dataStore.AddInitialData();
        return dataStore;
    });

    builder.Services.AddScoped<ICollectionRepository, InMemoryCollectionRepository>();
    builder.Services.AddScoped<ISectionRepository, InMemorySectionRepository>();
    builder.Services.AddScoped<IItemRepository, InMemoryItemRepository>();

    app = builder.Build();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var appState = services.GetRequiredService<AppState>();
    var dataStore = services.GetRequiredService<DataStore>();
    appState.UpdateCollectionCount(dataStore.Collections.Count);
    appState.UpdateSectionCount(dataStore.Sections.Count);
    appState.UpdateItemCount(dataStore.Items.Count);

    #endregion
}
else
{
    #region EF Core

    // BLL
    builder.Services.AddScoped<ICollectionManager, CollectionManager>();
    builder.Services.AddScoped<ISectionManager, SectionManager>();
    builder.Services.AddScoped<IItemManager, ItemManager>();

    // DAL
    // Source: https://learn.microsoft.com/en-us/aspnet/core/blazor/blazor-ef-core?view=aspnetcore-8.0#enable-sensitive-data-logging
#if DEBUG
    builder.Services.AddDbContextFactory<WeStockDbContext>(options =>
    {
        options.UseSqlite($"Data Source={nameof(WeStock)}.db");
        options.EnableSensitiveDataLogging();
    });
#else
    builder.Services.AddDbContextFactory<WeStockDbContext>(
        options => options.UseSqlite($"Data Source={nameof(WeStock)}.db")
    );
#endif

    builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
    builder.Services.AddScoped<ISectionRepository, SectionRepository>();
    builder.Services.AddScoped<IItemRepository, ItemRepository>();


    app = builder.Build();

    // !: Apply migrations and create the database at runtime if it doesn't exist
    // Source:
    // https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WeStockDbContext>();

    // Ensure the database is created and apply migrations only if needed.
    await context.Database.MigrateAsync();

    await SeedData.AddInitialDataAsync(context);

    var appState = services.GetRequiredService<AppState>();
    appState.UpdateCollectionCount(context.Collections.Count());
    appState.UpdateSectionCount(context.Sections.Count());
    appState.UpdateItemCount(context.Items.Count());

    #endregion
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
// app.UseStatusCodePagesWithRedirects("/404");

app.UseStaticFiles();
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();