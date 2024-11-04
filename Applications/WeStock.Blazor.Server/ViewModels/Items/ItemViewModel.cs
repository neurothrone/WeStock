using System.ComponentModel.DataAnnotations;

namespace WeStock.Blazor.Server.ViewModels.Items;

public class ItemViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Quantity has to be equal to or greater than 0")]
    public int Quantity { get; set; } = 0;

    public int SectionId { get; set; }

    public bool IsValid => !string.IsNullOrWhiteSpace(Name) && Quantity >= 0;

    // TODO: Note?

    // TODO: Price?, HasPrice?
    // [Range(typeof(decimal), "0", "9999")]
    // public decimal Price { get; set; }
}