using System.ComponentModel.DataAnnotations;

namespace WeStock.Blazor.Server.ViewModels.Collections;

public class CollectionViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    public bool IsValid => !string.IsNullOrWhiteSpace(Name);
}