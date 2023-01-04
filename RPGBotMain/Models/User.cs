using System;
using System.Data;
using System.Data.SqlClient;

namespace RPGBotMain.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int Xp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public int Money { get; set; }

        public User(SqlConnection con, string id)
        {
            DataTable dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter($"Select * from Users WHERE Id = '{id}'", con);
            adapter.Fill(dt);

            Id = id;
            Username = dt.Rows[0]["Username"].ToString();
            Xp = Convert.ToInt32(dt.Rows[0]["Xp"]);
        }
    }
}
