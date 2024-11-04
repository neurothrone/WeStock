# Requirements

### Test Data

Test data will be generated when first starting the application or whenever starting the app with no data.

### Search methods

- Both can be found in ItemRepository.
    - Path: `WeStock.DAL.EFCore/Repositories/ItemRepository`.
    - Method: `SearchItemsByNameAsync()`.
        - Used in `Items` page to search all items by name.
    - Method: `SearchItemsByNameExcludingSectionAsync()`.
        - Used in `SectionDetails` Page to search for all items that are not related to the current Section and filter
          by name.

```c#
// Path: WeStock.DAL.EFCore.Repositories/ItemRepository.cs
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
```

### Lambda Expressions

- Are used extensively over my entire solution.
- Mostly used in DAL, but can be found in other layers as well.

### Anonymous Delegates

- Can be found in `StatusBar.razor`.
- Path: `WeStock.Blazor.Server/Components/Features/Shared/Controls/StatusBar.razor`.

```c#
// Inside Appstate.cs:
public Action<int>? OnCollectionCountChanged { get; set; }
public Action<int>? OnSectionCountChanged { get; set; }
public Action<int>? OnItemCountChanged { get; set; }

// Inside StatusBar.razor:
protected override void OnAfterRender(bool firstRender)
{
    if (!firstRender)
        return;

    AppState.OnCollectionCountChanged = async collectionCount =>
    {
        _collectionCount = collectionCount;
        await InvokeAsync(StateHasChanged);
    };

    AppState.OnSectionCountChanged = async sectionCount =>
    {
        _sectionCount = sectionCount;
        await InvokeAsync(StateHasChanged);
    };

    AppState.OnItemCountChanged = async itemCount =>
    {
        _itemCount = itemCount;
        await InvokeAsync(StateHasChanged);
    };
}
```

### Anonymous Type

- Used in the method `SearchItemsByNameExcludingSectionAsync()` inside `InMemoryItemRepository`.

```c#
// Path: WeStock.DAL.InMemory.Repositories/InMemoryItemRepository.cs
public Task<IEnumerable<IItemEntity>> SearchItemsByNameExcludingSectionAsync(int sectionId, string? searchText)
{
    if (string.IsNullOrWhiteSpace(searchText))
        searchText = string.Empty;

    var excludedItemIds = _dataStore.ItemSectionMapping
        .Where(entry => entry.Value.Contains(sectionId))
        .Select(entry => entry.Key)
        .ToHashSet();

    var itemEntities = _dataStore.Items
        // Filter out all items that are related to the section to exclude.
        .Where(item => !excludedItemIds.Contains(item.Id))
        .Where(item => item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
        // Projecting to an anonymous type and excluding the SectionId property.
        .Select(item => new
        {
            item.Id,
            item.Name,
            item.Quantity
        })
        .ToList();

    return Task.FromResult<IEnumerable<IItemEntity>>(
        itemEntities.Select(item =>
            new InMemoryItemEntity
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity
            })
    );
}
```

### Advanced LINQ Queries

- Used in `CollectionDetails` Page to show total item count and react to changes.
- Used by `CollectionRepository` and `InMemoryCollectionRepository`.
- Filtering, Flattening, Distinct and Aggregation.

```c#
// Path: WeStock.DAL.EFCore/Repositories/CollectionRepository.cs
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
```

### Event Delegates and Advanced Event Handling

- I have used event delegates extensively throughout the PL.
    - To make more reusable components such as `ItemForm` and `SectionForm` as illustrated by the code examples below.
    - `ItemSearchBar` and `ItemSearchResults`.
        - Used by `Items` and `SectionSearchItems`.
- I have also used them to communicate between the PL and the SL, such as whenever one of the entities is created or
  removed.
    - Communication between `CollectionService`, `SectionService`, and `ItemService` in SL with `AppState` and
      `StatusBar`
      in PL.

#### Note

For most use cases where `Func` could have been used, `EventCallback` was used instead to leverage Blazor's built-in
event system in order to avoid re-rendering manually as illustrated by the examples below:

- Used on `ItemForm.razor.cs`.
    - `public EventCallback<ItemViewModel> OnItemCreated { get; set; }`
        - Equivalent to: `public Func<ItemViewModel, Task>? OnItemCreated { get; set; }`
    - `public EventCallback OnCancelClicked { get; set; }`
        - Equivalent to: `public Func<ItemViewModel, Task>? OnItemCreated { get; set; }`
- Same approach is used on `SectionForm.razor.cs`.
    - `public EventCallback<SectionViewModel> OnSectionCreated { get; set; }`

#### Examples

Using `EventCallback`:

```c#
// Path: WeStock.Blazor.Server/Components/Features/Shared/Controls/ItemFom.razor.cs
public partial class ItemForm
{
    [Parameter]
    public EventCallback<ItemViewModel> OnItemCreated { get; set; }

    [Parameter]
    public EventCallback OnCancelClicked { get; set; }

    private ItemViewModel _newItem = new();

    private async Task CreateItem()
    {
        if (OnItemCreated.HasDelegate)
            await OnItemCreated.InvokeAsync(_newItem);

        _newItem = new ItemViewModel();
    }

    private async Task Cancel()
    {
        if (OnCancelClicked.HasDelegate)
            await OnCancelClicked.InvokeAsync();
    }
}

// Path: WeStock.Blazor.Server/Components/Features/Items/Pages/Items.razor.cs
public partial class Items
{
    private async Task AddItem(ItemViewModel item)
    {
        var newItem = await ItemService.CreateItemAsync(item.MapToCreateDto(sectionId: 0));
        _items?.Add(newItem.MapToViewModel());
    }
}
```

Using `Func`:

```c#
// Path: WeStock.Blazor.Server/Components/Features/Shared/Controls/ItemFom.razor.cs
public partial class ItemForm
{
    [Parameter]
    public Func<ItemViewModel, Task>? OnItemCreated { get; set; }

    [Parameter]
    public Func<Task>? OnCancelClicked { get; set; }

    private ItemViewModel _newItem = new();

    private async Task CreateItem()
    {
        if (OnItemCreated != null)
            await OnItemCreated.Invoke(_newItem);

        _newItem = new ItemViewModel();
    }

    private async Task Cancel()
    {
        if (OnCancelClicked is not null)
            await OnCancelClicked.Invoke();
    }
}

// Path: WeStock.Blazor.Server/Components/Features/Items/Pages/Items.razor.cs
public partial class Items
{
    private async Task AddItem(ItemViewModel item)
    {
        var newItem = await ItemService.CreateItemAsync(item.MapToCreateDto(sectionId: 0));
        _items?.Add(newItem.MapToViewModel());
        StateHasChanged();
    }
}
```
