using System.ComponentModel.DataAnnotations;

namespace AuroraLibrary.DatabaseModels;

public class Item
{
    [Key]
    public int Id { get; set; }
    public ItemModel Model { get; set; }
    public Player Owner { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Value { get; set; }
    [Required]
    public bool Equipped { get; set; }
    public int Atttribute1Value { get; set; }
    public int Atttribute2Value { get; set; }
    public int Atttribute3Value { get; set; }
    public int Atttribute4Value { get; set; }
}

