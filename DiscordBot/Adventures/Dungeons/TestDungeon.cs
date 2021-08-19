using DiscordBot.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace DiscordBot.Adventures.Dungeons
{
    class TestDungeon: AdventureInterface
    {
        public override int ID { get; } = 1;
        public override string name { get; } = "Test Dungeon";
        public override int[] lootTable { get; } = { 1, 2 };
        public override int minDrops { get; } = 0;
        public override int maxDrops { get; } = 2;
        public override int recommendedLevel {get; } = 0;
        public override int recommendedEquipLevel {get; } = 0;
        /*
         * Wooden Sword
         * Broken Sword
         */

        public override TimeSpan dungeonTime {get;} = new TimeSpan(0, 0, 1);

        public override List<Item> Finish(MySqlConnection con, ulong userId)
        {
            User user = User.GetUserFromId(userId.ToString());

            var levelDifference = user.Level - recommendedLevel;
            var equipLevelDifference = user.GetEquipmentLevel() - recommendedEquipLevel;
            double percentPass = levelDifference + equipLevelDifference + 7.5;
            percentPass *= 10;

            Random pass = new Random();
            var passValue = pass.Next(0,101);

            // Failed
            if(passValue > percentPass)
            {
                return null;
            }
            //Passed
            List<Item> itemList = ItemIdToItems(lootTable, con);
            List<Item> dropList = new List<Item>();
            Random rdm = new Random();
            itemList[0].DropChance = 55;
            itemList[1].DropChance = 25;



            for(int i = 0; i < itemList.Count; i++)
            {
                var roll = rdm.Next(0, 101);

                itemList[i].RandomizeAllStats(-2,3);

                if(roll <= itemList[i].DropChance)
                {
                    dropList.Add(itemList[i]);
                }
            }

            return dropList;
        }
    }
}
