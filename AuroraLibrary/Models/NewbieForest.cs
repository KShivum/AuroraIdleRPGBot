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
        //TODO: This will crash as I need to fix the types. Probably make an item model for sword and wild herb and use ef to find the type it needs
        new ItemChance
        {
            Item = new Item
            {
                Name = "Wooden Sword",
                Description = "An old wooden sword",
                Model = db.ItemModels.First(x => x.Name.Equals("Wooden Sword")),
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
                Model = db.ItemModels.First(x => x.Name.Equals("Healing Grass")),
            },
            Chance = 35f
        },

        
    };

    public NewbieForest(RPGBotDBContext db) : base(db)
    {
    }
}

