using AuroraLibrary.DatabaseModels;

namespace AuroraLibrary.Models;

public class NewbieForest : AdventureBase
{
    public override string Name => "Newbie Forest";
    public override string Description => "You are in a forest. Many have traveled here before you, starting their adventures into the world";
    public override TimeSpan Duration => new TimeSpan(0, 0, 20, 0, 0);
    protected override List<ItemChance> DropableItems { get; set; } = new List<ItemChance>
    {
        new ItemChance
        {
            Item = new Item
            {
                Name = "Wooden Sword",
                Description = "An old wooden sword",
                Type = ItemTypes.Sword,
                Atttribute1Label = "Attack",
                Atttribute1Value = 3
            },
            Chance = 20f
        },
        new ItemChance
        {
            Item = new Item
            {
                Name = "Wild Herb",
                Description = "A wild herb commonly used in medicine",
                Type = ItemTypes.Material
            },
            Chance = 35f
        },

        
    };
}

