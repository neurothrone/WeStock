using System.ComponentModel.DataAnnotations;

namespace WeStock.Blazor.Server.ViewModels.Sections;

public class SectionViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    public int CollectionId { get; set; }
    
    public bool IsValid => !string.IsNullOrWhiteSpace(Name);
}