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
            CheckItemTable();
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

        public void CheckItemTable()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select TOP 1 * FROM Item", con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("Invalid object"))
                {
                    Console.WriteLine("Creating User Table");
                    SqlCommand cmd = new SqlCommand("CREATE TABLE [dbo].[Item]([Id] [int] IDENTITY(1,1) NOT NULL,[ItemName] [varchar](max) NOT NULL,[Stat1] [int] NULL,[Stat2] [int] NULL,[Stat3] [int] NULL,[Stat4] [int] NULL,[Equipped] [bit] NOT NULL,[Owner] [varchar](max) NULL)", con);
                    cmd.ExecuteNonQuery();
    
                }
            }

        }
    }
}
