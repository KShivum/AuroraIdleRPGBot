using AuroraLibrary.DatabaseModels;

namespace AuroraLibrary.Models
{
    public abstract class AdventureBase
    {
        public virtual int Id { get; protected set; } = 0;
        public virtual string Name { get; protected set; } = "";
        public virtual string Description { get; protected set; } = "";
        public virtual TimeSpan Duration { get; protected set; }
        public virtual List<ItemChance> DropableItems { get; protected set; } = new List<ItemChance>();
    }

    public class ItemChance
    {
        public Item Item { get; set; }
        public float Chance { get; set; }
    }
}
