using System.ComponentModel.DataAnnotations;

namespace AuroraLibrary.DatabaseModels;

public class Recipe
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public ItemModel CreatedItem { get; set; }
    
    [Required]
    public List<ItemModel> Materials { get; set; }

    [Required] public int XPRequired;
}