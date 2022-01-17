using System;
using System.Collections.Generic;

namespace RPGBotMain.Models
{
    public class AdventureType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Event[] AttackEvents {get; set;}
        public Event[] GatherEvents {get; set;}

        //!Overwrite these!
        public List<Item> GetLoot() {return null;}
        public TimeSpan GetAdventureLength() {return new TimeSpan();}

    }
}
