using System;
using System.Data.SqlClient;

namespace RPGBotMain
{
    public class DatabaseManager
    {
        public SqlConnection con;

        string UserCreationString = @"CREATE TABLE [dbo].[Users](
	[Id] [varchar](255) NULL,
	[PlayerName] [varchar](255) NULL,
	[Xp] [int] NULL,
	[Attack] [int] NOT NULL,
	[Defense] [int] NOT NULL,
	[Speed] [int] NOT NULL,
	[Money] [int] NOT NULL
)";


        public DatabaseManager(SqlConnection conn)
        {
            con = conn;
            CheckUserTable();
            CheckItemTable();
            CheckAdventureTable();


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
                    SqlCommand cmd = new SqlCommand(UserCreationString, con);
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
                    SqlCommand cmd = new SqlCommand("CREATE TABLE [dbo].[Item]([Id] [int] IDENTITY(1,1) NOT NULL,[ItemName] [varchar](max) NOT NULL, [ItemType] [varchar](max) NOT NULL, [Stat1] [int] NULL,[Stat2] [int] NULL,[Stat3] [int] NULL,[Stat4] [int] NULL,[Equipped] [bit] NOT NULL,[Owner] [varchar](max) NULL)", con);
                    cmd.ExecuteNonQuery();
    
                }
            }

        }

        public void CheckAdventureTable()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select TOP 1 * FROM Adventure", con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("Invalid object"))
                {
                    Console.WriteLine("Creating Adventure Table");
                    SqlCommand cmd = new SqlCommand("CREATE TABLE [dbo].[Adventure]([Id] [int] IDENTITY(1,1) NOT NULL,[Player] [varchar](max) NOT NULL,[AdventureType] [int] NOT NULL,[TimeStart] [datetime] NOT NULL,[TimeEnd] [datetime] NULL,[Active] [bit] NOT NULL)", con);
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
