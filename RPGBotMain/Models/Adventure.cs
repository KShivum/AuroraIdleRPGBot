using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RPGBotMain.Adventures;

namespace RPGBotMain.Models
{
    public class Adventure
    {
        public int Id { get; set; }
        public string Player { get; set; }
        public int AdventureId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Active { get; set; }

        

        private static List<AdventureType> allAdventures;





        public static Adventure GetPlayerActiveAdventure(ulong player, SqlConnection con)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Adventure WHERE Player = @player AND Active = 1", con))
            {
                cmd.Parameters.AddWithValue("@player", player);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Adventure
                        {
                            Id = reader.GetInt32(0),
                            Player = reader.GetString(1),
                            AdventureId = reader.GetInt32(2),
                            StartTime = reader.GetDateTime(3),
                            EndTime = reader.GetDateTime(4),
                            Active = reader.GetBoolean(5)
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static List<AdventureType> GetAdventures()
        {
            if(allAdventures == null)
            {
                allAdventures = new List<AdventureType>();
                allAdventures.Add(new NoobieForest());                
            }
            
            return allAdventures;
        }

        
    }
}
