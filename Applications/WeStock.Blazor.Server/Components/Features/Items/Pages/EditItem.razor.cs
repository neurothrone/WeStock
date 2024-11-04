using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Items;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Items.Pages;

public partial class EditItem
{
    [SupplyParameterFromQuery(Name = "id")]
    private int ItemId { get; set; }

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Inject]
    private IItemService ItemService { get; set; } = null!;

    private ItemViewModel? _item;

    protected override async Task OnInitializedAsync()
    {
        var itemDto = await ItemService.RetrieveItemByIdAsync(ItemId);
        if (itemDto is null)
        {
            Navigation.NavigateTo("/404");
            return;
        }

        _item = itemDto.MapToViewModel();
    }

    private async Task UpdateItem()
    {
        if (_item is null)
            return;

        var updated = await ItemService.UpdateItemAsync(_item.MapToDto());
        if (updated)
            await NavigateBack();

        // TODO: else show error alert
    }

    private async Task NavigateBack()
    {
        await JsRuntime.InvokeVoidAsync("goBack");
    }
}