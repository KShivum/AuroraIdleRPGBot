using System.ComponentModel.DataAnnotations;

namespace AuroraLibrary.DatabaseModels;

public class ItemModel
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public ItemTypes Type { get; set; }
    public Rarities Rarity { get; set; } = Rarities.Common;
    public string? Atttribute1Label { get; set; }
    public string? Atttribute2Label { get; set; }
    public string? Atttribute3Label { get; set; }
    public string? Atttribute4Label { get; set; }
}

public enum ItemTypes
{
    Material,
    Sword,
    Consumable
}

public enum Rarities
{
    Trash,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Unique,
    Mythic
}