using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.ViewModels.Sections;

namespace WeStock.Blazor.Server.Components.Features.Shared.Controls;

public partial class SectionForm
{
    [Parameter]
    public EventCallback<SectionViewModel> OnSectionCreated { get; set; }

    private SectionViewModel _newSection = new();

    private async Task CreateSection()
    {
        if (OnSectionCreated.HasDelegate)
            await OnSectionCreated.InvokeAsync(_newSection);

        _newSection = new SectionViewModel();
        // TODO: set focus back on name field
    }
}