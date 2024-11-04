using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.Components.Features.Collections.Controls;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Items;
using WeStock.Blazor.Server.ViewModels.Sections;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Sections.Pages;

public partial class SectionDetails
{
    [SupplyParameterFromQuery(Name = "id")]
    private int SectionId { get; set; }

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Inject]
    private ISectionService SectionService { get; set; } = null!;

    private SectionViewModel? _section;
    private ItemList _itemList = null!;

    protected override async Task OnInitializedAsync()
    {
        var sectionDto = await SectionService.RetrieveSectionByIdAsync(SectionId);
        if (sectionDto is null)
        {
            Navigation.NavigateTo("/404");
            return;
        }

        _section = sectionDto.MapToViewModel();
    }

    private async Task AddItemToSection(ItemViewModel item)
    {
        await _itemList.AddItemToSection(item);
    }

    private async Task DeleteItem(ItemViewModel item)
    {
        await _itemList.DeleteItem(item);
    }
}