using System;
using System.Collections.Generic;

namespace RPGBotMain.Models
{
    public class AdventureType
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int AttackEventCount { get; set; }
        public virtual int AttackEventDeviation { get; set; }
        public virtual Event[] AttackEvents {get; set;}
        public virtual int GatherEventCount { get; set; }
        public virtual int GatherEventDeviation { get; set; }
        public virtual Event[] GatherEvents {get; set;}

        //!Overwrite these!
        public List<Item> GetLoot() {return null;}
        public TimeSpan GetAdventureLength() {return new TimeSpan();}

    }
}
