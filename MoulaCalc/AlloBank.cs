using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Data;

namespace MoulaCalc
{
    public class AlloBank
    {
        DatabaseManager databaseManager = new();

        private long Billet5 { get; set; }
        private long Billet10 { get; set; }
        private long Billet20 { get; set; }
        private long Billet50 { get; set; }
        private long Billet100 { get; set; }
        private long Billet200 { get; set; }
        private long Billet500 { get; set; }
        private string Monnaie { get; set; }
        public List<string> Total { get; set; }
        public List<string> Date { get; set; }

        public long GetBillet5()
        {
            return Billet5;
        }
        public long GetBillet10()
        {
            return Billet10;
        }
        public long GetBillet20()
        {
            return Billet20;
        }
        public long GetBillet50()
        {
            return Billet50;
        }
        public long GetBillet100()
        {
            return Billet100;
        }
        public long GetBillet200()
        {
            return Billet200;
        }
        public long GetBillet500()
        {
            return Billet500;
        }
        public string GetMonnaie()
        {
            return Monnaie;
        }
        public List<string> GetTotal()
        {
            return Total;
        }
        public List<string> GetDate()
        {
            return Date;
        }

        public void InsertNbBillets(long Billet5, long Billet10, long Billet20, long Billet50, long Billet100, long Billet200, long Billet500, string Monnaie)
        {
            string insert = "INSERT INTO Encours(Billet5,Billet10,Billet20,Billet50,Billet100,Billet200,Billet500,Monnaie) VALUES(" + Billet5 + "," + Billet10 + "," + Billet20 + "," + Billet50 + "," + Billet100 + "," + Billet200 + "," + Billet500 + ",'" + Monnaie + "')";
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            databaseManager.executeQuery(insert);
            databaseManager.closeDbConnection();

        }

        public void InsertAlloBank(string Total, string Date, long Billet5, long Billet10, long Billet20, long Billet50, long Billet100, long Billet200, long Billet500)
        {
            string insert = "INSERT INTO AlloBank(Total,Date,Billet5,Billet10,Billet20,Billet50,Billet100,Billet200,Billet500) VALUES(" + Total + ",'"+ Date + "'," + Billet5 + "," + Billet10 + "," + Billet20 + "," + Billet50 + "," + Billet100 + "," + Billet200 + "," + Billet500 + ")";
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            databaseManager.executeQuery(insert);
            databaseManager.closeDbConnection();
        }

        public void SelectAllAlloBank()
        {
            string select = "SELECT Total, Date FROM AlloBank";
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            SQLiteDataReader reader = databaseManager.executeSelectQuery(select);
            Total = new();
            Date = new();
            while (reader.Read())
            {
                Total.Add(reader["Total"].ToString());
                Date.Add(reader["Date"].ToString());
            }
            reader.Close();

            databaseManager.closeDbConnection();
        }

        public void SelectAlloBankByTotal(string VarTotal)
        {
            string select = "SELECT Total, Date FROM AlloBank WHERE Total=" + VarTotal;
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            SQLiteDataReader reader = databaseManager.executeSelectQuery(select);
            Total = new();
            Date = new();
            while (reader.Read())
            {
                Total.Add(reader["Total"].ToString());
                Date.Add(reader["Date"].ToString());
            }
            reader.Close();

            databaseManager.closeDbConnection();
        }

            public void UpdateAlloBank()
        {

        }

        public void DeleteAlloBank()
        {
            string cmd = "DELETE FROM AlloBank";
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            databaseManager.executeQuery(cmd);
            string cmd2 = "DELETE FROM Encours";
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            databaseManager.executeQuery(cmd2);
            databaseManager.closeDbConnection();
        }

        public void DeleteNbBillets()
        {
            string cmd = "DELETE FROM Encours";
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            databaseManager.executeQuery(cmd);
            databaseManager.closeDbConnection();
        }

        public void DeleteByRowID(int rowID)
        {
            string cmd = "DELETE FROM AlloBank WHERE Id="+ rowID;
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            databaseManager.executeQuery(cmd);
            databaseManager.closeDbConnection();
        }
    }
}
