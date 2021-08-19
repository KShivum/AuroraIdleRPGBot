using DiscordBot.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DiscordBot.Adventures
{
    abstract class AdventureInterface
    {
        public abstract int ID { get; }
        public abstract string name { get; }
        public abstract int[] lootTable { get; }
        public abstract int minDrops { get; }
        public abstract int maxDrops { get; }
        public abstract int recommendedLevel {get;}
        public abstract int recommendedEquipLevel {get;}
        public abstract List<Item> Finish(MySqlConnection con, ulong userId);
        public abstract TimeSpan dungeonTime {get;}

        //TODO: add Embeds that

        public List<Item> ItemIdToItems(int[] ItemIds, MySqlConnection con)
        {
            DataSet itemDataset = new DataSet();
            string command = "Select * from itemList Where ";

            //We will add on to a string and grab all items with id
            for(int i = 0; i < ItemIds.Length - 1; i++)
            {
                command = command + $"ID = {ItemIds[i]} OR ";
            }

            //Just adding the last item
            command = command + $"ID = {ItemIds[ItemIds.Length - 1]}";

            MySqlDataAdapter adapter = new MySqlDataAdapter(command, con);

            adapter.Fill(itemDataset);

            List<Item> itemList = new List<Item>();
            for(int i = 0; i < itemDataset.Tables[0].Rows.Count; i++)
            {
                var temp = new Item();

                temp.ID = Convert.ToInt32(itemDataset.Tables[0].Rows[i]["ID"]);
                temp.ItemType = itemDataset.Tables[0].Rows[i]["ItemType"].ToString();
                temp.Name =  itemDataset.Tables[0].Rows[i]["ItemName"].ToString();

                //We might need to convert those ints to nullable ints or check, which I will probably do
                if(itemDataset.Tables[0].Rows[i]["BaseStat1"] != DBNull.Value) 
                    temp.Stat1 =  Convert.ToInt32(itemDataset.Tables[0].Rows[i]["BaseStat1"]);
                if(itemDataset.Tables[0].Rows[i]["BaseStat2"] != DBNull.Value)
                    temp.Stat2 = Convert.ToInt32(itemDataset.Tables[0].Rows[i]["BaseStat2"]);
                if(itemDataset.Tables[0].Rows[i]["BaseStat3"] != DBNull.Value)
                    temp.Stat3 = Convert.ToInt32(itemDataset.Tables[0].Rows[i]["BaseStat3"]);
                if(itemDataset.Tables[0].Rows[i]["BaseStat4"] != DBNull.Value)
                    temp.Stat4 = Convert.ToInt32(itemDataset.Tables[0].Rows[i]["BaseStat4"]);
                itemList.Add(temp);

            }

            return itemList;
        }
        
    }
}
