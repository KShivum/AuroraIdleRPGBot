using DSharpPlus.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static DiscordBot.GlobalItems;

namespace DiscordBot.Models
{
    class Item
    {
        public int? ID { get; set; }
        public string ItemType { get; set; }
        public int? Stat1 { get; set; }
        public int? Stat2 { get; set; }
        public int? Stat3 { get; set; }
        public int? Stat4 { get; set; }
        public string Owner { get; set; }
        public bool Equipped { get; set; } = false;
        public string Name { get; set; } = "Default Name";

        public int? DropChance { get; set; }

        public Item()
        { }

        public Item(int id, string itemType, int? stat1, int? stat2, int? stat3, int? stat4, string owner, bool equipped, string name)
        {
            ID = id;
            ItemType = itemType;
            Stat1 = stat1;
            Stat2 = stat2;
            Stat3 = stat3;
            Stat4 = stat4;
            Owner = owner;
            Equipped = equipped;
            Name = name;

        }

        public Item(int id, string itemType, int? stat1, int? stat2, int? stat3, int? stat4, string owner, bool equipped, string name, int dropChance)
        {
            ID = id;
            ItemType = itemType;
            Stat1 = stat1;
            Stat2 = stat2;
            Stat3 = stat3;
            Stat4 = stat4;
            Owner = owner;
            Equipped = equipped;
            Name = name;
            DropChance = dropChance;

        }
        public Item(int id, string itemType, int stat1, int stat2, int stat3, int stat4, string name)
        {
            ID = id;
            ItemType = itemType;
            Stat1 = stat1;
            Stat2 = stat2;
            Stat3 = stat3;
            Stat4 = stat4;
            Name = name;
        }


        public bool AddItem(MySqlConnection con)
        {
            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand($"Insert Into items (ItemType, Stat1, Stat2, Stat3, Stat4, Owner, Equipped, Name) Values ('{ItemType}', {Stat1}, {Stat2}, {Stat3}, {Stat4}, '{Owner}', {Equipped}, '{Name}')", con);
                query.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

        }

        public DiscordEmbedBuilder GetItemStats()
        {
            
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{Name}",
                Description = $"{ItemType}",
            };

            embed.WithFooter($"ID: {ID}");
            //TODO: Add a different fields depending on itemtype
            embed.AddField("Stats", $"Stat 1:  {Stat1}       Stat 2: {Stat2}       Stat 3: {Stat3}       Stat 2: {Stat3}");

            if(Equipped)
            {
                embed.AddField("Equipped:", ":white_check_mark:");
            }
            else
            {
                embed.AddField("Equipped:", ":x:");
            }

            return embed;
        }

        public string GetItemStatString()
        {
            string text;
            if(ItemType.Equals("Sword"))
            {
                text = $":dagger:     Damage: {Stat1}   Defense: {Stat2}   Speed: {Stat3}   /shrug: {Stat4}   ";
                if(Equipped)
                {
                    text += $":white_check_mark:";
                }
                else
                {
                    text += $":x:";
                }

                return text;
            }

            return null;
        }

        public string GetItemStatStringWithID()
        {
            

            return GetItemStatString() + $"\t ID: {ID}";
        }

        

        //Both values are inclusive
        public void RandomizeAllStats(int min, int max)
        {
            Random rdm = new Random();
            //Rolling random stats
            if(Stat1 != null)
                Stat1 += rdm.Next(min, max+1);
            if(Stat2 != null)
                Stat2 += rdm.Next(min, max+1);
            if(Stat3 != null)
                Stat3 += rdm.Next(min, max+1);
            if(Stat4 != null)
                Stat4 += rdm.Next(min, max+1);

            if(Stat1 < 0 && Stat1 != null)
                Stat1 = 0;
            if(Stat2 < 0 && Stat2 != null)
                Stat2 = 0;
            if(Stat3 < 0 && Stat3 != null)
                Stat3 = 0;
            if(Stat4 < 0 && Stat4 != null)
                Stat4 = 0;
        }

        public int GetItemLevel()
        {
            int count = 0;
            if(Stat1 != null)
            {
                count += (int)Stat1;
            }
            if(Stat2 != null)
            {
                count += (int)Stat2;
            }
            if(Stat3 != null)
            {
                count += (int)Stat3;
            }
            if(Stat4 != null)
            {
                count += (int)Stat4;
            }

            return count;
        }

        public static Item GetItemFromId(int itemId)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter($"Select * From items where ItemID = {itemId}", ConString);
            DataSet item = new DataSet();
            adapter.Fill(item);
            int? stat1, stat2, stat3, stat4;


            if(DBNull.Value == item.Tables[0].Rows[0]["Stat1"])
            {
                stat1 = null;
            }
            else
            {
                stat1 = Convert.ToInt32(item.Tables[0].Rows[0]["Stat1"]);
            }

            if(DBNull.Value ==item.Tables[0].Rows[0]["Stat2"])
            {
                stat2 = null;
            }
            else
            {
                stat2 = Convert.ToInt32(item.Tables[0].Rows[0]["Stat2"]);
            }

            if(DBNull.Value == item.Tables[0].Rows[0]["Stat3"])
            {
                stat3 = null;
            }
            else
            {
                stat3 = Convert.ToInt32(item.Tables[0].Rows[0]["Stat3"]);
            }

            if(DBNull.Value == item.Tables[0].Rows[0]["Stat4"])
            {
                stat4 = null;
            }
            else
            {
                stat4 = Convert.ToInt32(item.Tables[0].Rows[0]["Stat4"]);
            }

            return new Item(Convert.ToInt32(item.Tables[0].Rows[0]["ItemID"]), item.Tables[0].Rows[0]["ItemType"].ToString(),stat1 , stat2,
                        stat3 ,stat4, item.Tables[0].Rows[0]["Owner"].ToString(), Convert.ToBoolean(item.Tables[0].Rows[0]["Equipped"]), item.Tables[0].Rows[0]["Name"].ToString());
        }


    }
}
