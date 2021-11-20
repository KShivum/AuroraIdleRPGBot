using System;
using System.Data.SqlClient;

namespace RPGBotMain
{
    public class DatabaseManager
    {
        public SqlConnection con;
        public DatabaseManager(SqlConnection conn)
        {
            con = conn;
            CheckUserTable();
        }

        public void CheckUserTable()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select TOP 1 * FROM Users", con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("Invalid object"))
                {
                    Console.WriteLine("Creating User Table");
                    SqlCommand cmd = new SqlCommand("CREATE TABLE Users(Id varchar(255),PlayerName varchar(255),Xp int)", con);
                    cmd.ExecuteNonQuery();
                }
            }


        }
    }
}
