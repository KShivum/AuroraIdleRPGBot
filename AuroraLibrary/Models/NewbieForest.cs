using AuroraLibrary.DatabaseModels;

namespace AuroraLibrary.Models;

public class NewbieForest : AdventureBase
{
    public override int Id => 1;
    public override string Name => "Newbie Forest";
    public override string Description => "Many have traveled here before you, starting their adventures into the world";
    public override TimeSpan Duration => new TimeSpan(0, 0, 1, 0, 0);
    public override int StrengthRecommended => 10;
    public override int SpeedRecommended => 10;
    public override List<ItemChance> DropableItems { get; protected set; } = new List<ItemChance>
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

