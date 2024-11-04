using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Collections;
using WeStock.Blazor.Server.ViewModels.Items;
using WeStock.Blazor.Server.ViewModels.Sections;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Collections.Controls;

public partial class SectionList
{
    [Parameter, EditorRequired]
    public required CollectionViewModel Collection { get; set; }

    [Inject]
    private ISectionService SectionService { get; set; } = null!;

    private SectionViewModel? _selectedSection;
    private List<SectionViewModel>? _sections;
    private Dictionary<int, ItemList> _itemListReferences = [];

    protected override async Task OnInitializedAsync()
    {
        _sections = (await SectionService.RetrieveSectionsByCollectionIdAsync(Collection.Id))
            .Select(sectionDto => sectionDto.MapToViewModel())
            .ToList();
    }

    private async Task AddSection(SectionViewModel section)
    {
        var newSection = await SectionService.CreateSectionAsync(section.MapToCreateDto(collectionId: Collection.Id));
        _sections?.Add(newSection.MapToViewModel());
    }

    private async Task DeleteSection(SectionViewModel section)
    {
        var deleted = await SectionService.DeleteSectionByIdAsync(section.Id);
        if (!deleted)
            return;

        _sections?.Remove(section);
    }

    private async Task AddItem(ItemViewModel item)
    {
        if (_selectedSection is null)
            return;

        // Find the correct ItemList component to add the Item to.
        if (_itemListReferences.TryGetValue(_selectedSection.Id, out var itemList))
            await itemList.AddItemToSection(item);
    }
}