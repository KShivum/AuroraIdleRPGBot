using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using static DiscordBot.GlobalItems;
namespace DiscordBot.Models
{
    class User
    {
        public string ID { get; set; }
        public int XP { get; set; }
        public int Level {get; set;}
        public string PlayerName { get; set; }
        public int Money { get; set; }
        public Item EquippedWeapon {get; set;}
        public Item EquippedChestplate {get; set;}
        public Item EquippedLeggings {get; set;}

        public static User GetUserFromId(string userId)
        {
            DataSet user = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter($"Select * from users Where id = {userId}",ConString);
            adapter.Fill(user);

            User tempUser = new User();

            tempUser.ID = user.Tables[0].Rows[0]["id"].ToString();
            tempUser.XP = Convert.ToInt32(user.Tables[0].Rows[0]["xp"]);
            tempUser.Level = Convert.ToInt32(user.Tables[0].Rows[0]["currentLevel"]);
            tempUser.PlayerName = user.Tables[0].Rows[0]["playerName"].ToString();
            tempUser.Money = Convert.ToInt32(user.Tables[0].Rows[0]["money"]);

            if(DBNull.Value == user.Tables[0].Rows[0]["equippedWeapon"])
            {
                tempUser.EquippedWeapon = null;
            }
            else
            {
                tempUser.EquippedWeapon = Item.GetItemFromId(Convert.ToInt32(user.Tables[0].Rows[0]["equippedWeapon"]));
            }

            if(DBNull.Value == user.Tables[0].Rows[0]["equippedChestplate"])
            {
                tempUser.EquippedChestplate = null;
            }
            else
            {
                tempUser.EquippedChestplate = Item.GetItemFromId(Convert.ToInt32(user.Tables[0].Rows[0]["equippedChestplate"]));
            }

             if(DBNull.Value == user.Tables[0].Rows[0]["equippedLeggings"])
            {
                tempUser.EquippedLeggings = null;
            }
            else
            {
                tempUser.EquippedLeggings = Item.GetItemFromId(Convert.ToInt32(user.Tables[0].Rows[0]["equippedLeggings"]));
            }

            return tempUser;

        }

        public int GetEquipmentLevel()
        {
            int count = 0;

            if(EquippedWeapon != null)
                count += EquippedWeapon.GetItemLevel();

            if(EquippedChestplate != null)
                count += EquippedChestplate.GetItemLevel();

            if(EquippedLeggings != null)
                count += EquippedLeggings.GetItemLevel();
                
            return count;
        }
    }
}
