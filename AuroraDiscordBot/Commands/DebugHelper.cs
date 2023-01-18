using AuroraLibrary.DatabaseModels;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace AuroraDiscordBot.Commands;

public class DebugHelper : BaseCommandModule
{
    public RPGBotDBContext db { private get; set; }

    [Command("createItemModel")]
    public async Task CreateItemModel(CommandContext ctx, string itemType, string name, string rarity)
    {
        ItemTypes type;
        switch (itemType)
        {
            case "Material":
                type = ItemTypes.Material;
                break;
            case "Sword":
                type = ItemTypes.Sword;
                break;
            case "Consumable":
                type = ItemTypes.Consumable;
                break;
            default:
                var invalid = await ctx.RespondAsync("Invalid Item type");
                await Task.Delay(2000);
                await ctx.Channel.DeleteMessageAsync(invalid);
                return;
        }

        Rarities rarityEnum;

        switch (rarity)
        {
            case "Trash":
                rarityEnum = Rarities.Trash;
                break;
            case "Common":
                rarityEnum = Rarities.Common;
                break;
            case "Uncommon":
                rarityEnum = Rarities.Uncommon;
                break;
            case "Rare":
                rarityEnum = Rarities.Rare;
                break;
            case "Epic":
                rarityEnum = Rarities.Epic;
                break;
            case "Legendary":
                rarityEnum = Rarities.Legendary;
                break;
            case "Unique":
                rarityEnum = Rarities.Unique;
                break;
            case "Mythic":
                rarityEnum = Rarities.Mythic;
                break;
            default:
                var invalid = await ctx.RespondAsync("Invalid Item type");
                await Task.Delay(2000);
                await ctx.Channel.DeleteMessageAsync(invalid);
                return;
        }

        var model = new ItemModel()
        {
            Type = type,
            Name = name
        };

        await db.ItemModels.AddAsync(model);
        await db.SaveChangesAsync();

        var response = await ctx.RespondAsync("Added Model");
        await Task.Delay(2000);
        await ctx.Channel.DeleteMessageAsync(response);
    }


    //! Keep this at the bottom
    [Command("AddPremadeItems")]
    public async Task AddPremadeItems(CommandContext ctx)
    {
        List<ItemModel> models = new List<ItemModel>();
        List<Recipe> recipes = new List<Recipe>();

        var existingModels = db.ItemModels.ToList();

        //Instead of passing nameless types, it might be better to make each thing a variable

        #region Models

        var mHealingGrass = new ItemModel()
        {
            Name = "Healing Grass",
            Type = ItemTypes.Material
        };
        models.Add(mHealingGrass);

        var mWood = new ItemModel()
        {
            Name = "Wood",
            Type = ItemTypes.Material
        };
        models.Add(mWood);

        var mHealthPotion = new ItemModel()
        {
            Name = "Healing Potion",
            Type = ItemTypes.Consumable
        };
        models.Add(mHealthPotion);

        var mWoodSword = new ItemModel()
        {
            Name = "Wooden Sword",
            Type = ItemTypes.Sword,
            Atttribute1Label = "Attack"
        };
        models.Add(mWoodSword);

        #endregion

        #region Recipes

        var rHealthPotion = new Recipe()
        {
            CreatedItem = mHealthPotion,
            XPRequired = 0
        };
        rHealthPotion.Materials.Add(mHealingGrass);
        rHealthPotion.Materials.Add(mHealingGrass);
        recipes.Add(rHealthPotion);

        var rWoodSword = new Recipe()
        {
            CreatedItem = mWoodSword,
            XPRequired = 0
        };
        rWoodSword.Materials.Add(mWood);
        rWoodSword.Materials.Add(mWood);
        rWoodSword.Materials.Add(mWood);
        recipes.Add(rWoodSword);

        #endregion


        // Find any item models that are not in the database
        var newModels = models.Where(x => !existingModels.Any(y => y.Name == x.Name)).ToList();
        var newRecipes = recipes.Where(x => !existingModels.Any(y => y.Name == x.CreatedItem.Name)).ToList();

        // Add the new models to the database
        await db.ItemModels.AddRangeAsync(newModels);
        await db.ItemRecipies.AddRangeAsync(newRecipes);


        var reponse = await ctx.RespondAsync($"Added {newModels.Count} models and {newRecipes.Count} recipes");
        await Task.Delay(2000);
        await ctx.Channel.DeleteMessageAsync(reponse);
    }
}