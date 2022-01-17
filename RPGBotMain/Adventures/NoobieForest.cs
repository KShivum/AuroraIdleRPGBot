using System;
using System.Collections.Generic;
using RPGBotMain.Events;
using RPGBotMain.Models;

namespace RPGBotMain.Adventures
{
    public class NoobieForest : AdventureType
    {
        List<Item> loot = new List<Item>();
        TimeSpan adventureLength = new TimeSpan(0,0,15);
        public new Event[] AttackEvents {get; set;} =
        {
            new GenericAttackEvent("Goblin", 1, 2, 3),
            new GenericAttackEvent("Goblin", 1, 1, 2),
            new GenericAttackEvent("Ogre", 3, 2, 1)
            
        };
        
        public new Event[] GatherEvents {get; set; } = 
        {


        };
        



        public NoobieForest()
        {
            //Generate Loot
            loot.Add(new Item("Broken Iron Sword", "Sword", 1, 0, 2, null));

            
        }


        public List<Item> GetLoot()
        {
            return loot;
        }
        public TimeSpan GetAdventureLength()
        {
            return adventureLength;
        }


    }
}
