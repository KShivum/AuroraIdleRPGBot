using AuroraLibrary.DatabaseModels;

namespace AuroraLibrary.Models
{
    public abstract class AdventureBase
    {
        public virtual string Name { get; protected set; } = "";
        public virtual string Description { get; protected set; } = "";
        public virtual TimeSpan Duration { get; protected set; }
        protected virtual List<ItemChance> DropableItems { get; set; } = new List<ItemChance>();
    }

    public class ItemChance
    {
        public Item Item { get; set; }
        public float Chance { get; set; }
    }
}
