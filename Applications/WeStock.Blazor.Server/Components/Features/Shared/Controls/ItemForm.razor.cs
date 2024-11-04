using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.ViewModels.Items;

namespace WeStock.Blazor.Server.Components.Features.Shared.Controls;

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

        // TODO: set focus back on name field
    }

    private async Task Cancel()
    {
        if (OnCancelClicked.HasDelegate)
            await OnCancelClicked.InvokeAsync();
    }
}