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

        private List<string> Total { get; set; }
        private List<string> Date { get; set; }

        public List<string> GetTotal()
        {
            return Total;
        }
        public List<string> GetDate()
        {
            return Date;
        }

        public void InsertAlloBank(string Total, string Date)
        {
            string insert = "INSERT INTO AlloBank(Total,Date) VALUES("+ Total + ",'"+ Date + "')";
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
            string cmd = "DELETE FROM AlloBank;";
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            databaseManager.executeQuery(cmd);
            databaseManager.closeDbConnection();
        }

        public void DeleteByRowID(string rowID)
        {
            string cmd = "DELETE FROM AlloBank WHERE Id="+ rowID;
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            databaseManager.executeQuery(cmd);
            databaseManager.closeDbConnection();
        }
    }
}
