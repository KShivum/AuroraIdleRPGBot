using System;
using System.Data;
using System.Data.SqlClient;

namespace RPGBotMain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Stat1 { get; set; }
        public int? Stat2 { get; set; }
        public int? Stat3 { get; set; }
        public int? Stat4 { get; set; }
        public bool IsEquipped { get; set; }
        public string Owner { get; set; }


        public Item(string name, int? stat1, int? stat2, int? stat3, int? stat4, bool isEquipped, string owner)
        {
            Name = name;
            Stat1 = stat1;
            Stat2 = stat2;
            Stat3 = stat3;
            Stat4 = stat4;
            IsEquipped = isEquipped;
            Owner = owner;
        }
        public Item()
        {
        }

        public Item(SqlConnection con, int id)
        {
            DataTable dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM Item WHERE Id = {id}", con);
            adapter.Fill(dt);

            Id = id;
            Name = dt.Rows[0]["Name"].ToString();
            if (dt.Rows[0]["Stat1"] != DBNull.Value)
                Stat1 = Convert.ToInt32(dt.Rows[0]["Stat1"]);
            if (dt.Rows[0]["Stat2"] != DBNull.Value)
                Stat2 = Convert.ToInt32(dt.Rows[0]["Stat2"]);
            if (dt.Rows[0]["Stat3"] != DBNull.Value)
                Stat3 = Convert.ToInt32(dt.Rows[0]["Stat3"]);
            if (dt.Rows[0]["Stat4"] != DBNull.Value)
                Stat4 = Convert.ToInt32(dt.Rows[0]["Stat4"]);

            IsEquipped = Convert.ToBoolean(dt.Rows[0]["IsEquipped"]);
            Owner = dt.Rows[0]["Owner"].ToString();
        }



        public void Create(SqlConnection con)
        {
            string query = $"INSERT INTO Item (ItemName, Stat1, Stat2, Stat3, Stat4, Equipped, Owner) VALUES ('{Name}',";
            if (Stat1 != null)
                query += $"{Stat1},";
            else
                query += "NULL,";

            if (Stat2 != null)
                query += $"{Stat2},";
            else
                query += "NULL,";

            if (Stat3 != null)
                query += $"{Stat3},";
            else
                query += "NULL,";

            if (Stat4 != null)
                query += $"{Stat4},";
            else
                query += "NULL,";


            if (IsEquipped)
                query += "1,";
            else
                query += "0,";

            query += $"'{Owner}')";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }

        public void GetItemFromID(int Id, SqlConnection con)
        {

            DataTable dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM Item WHERE Id = {Id}", con);
            adapter.Fill(dt);

            this.Id = Id;
            Name = dt.Rows[0]["Name"].ToString();
            if (dt.Rows[0]["Stat1"] != DBNull.Value)
                Stat1 = Convert.ToInt32(dt.Rows[0]["Stat1"]);
            if (dt.Rows[0]["Stat2"] != DBNull.Value)
                Stat2 = Convert.ToInt32(dt.Rows[0]["Stat2"]);
            if (dt.Rows[0]["Stat3"] != DBNull.Value)
                Stat3 = Convert.ToInt32(dt.Rows[0]["Stat3"]);
            if (dt.Rows[0]["Stat4"] != DBNull.Value)
                Stat4 = Convert.ToInt32(dt.Rows[0]["Stat4"]);

            IsEquipped = Convert.ToBoolean(dt.Rows[0]["IsEquipped"]);
            Owner = dt.Rows[0]["Owner"].ToString();

        }
    }


}
