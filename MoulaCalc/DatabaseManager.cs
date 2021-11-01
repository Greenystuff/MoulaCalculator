using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace MoulaCalc
{
    public class DatabaseManager
    {
        SQLiteConnection dbConnection;
        SQLiteCommand command;
        string sqlCommand;
        string dbPath = Environment.CurrentDirectory + "\\DB";
        string dbFilePath;

        public void createDbFile()
        {
            if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }

            dbFilePath = dbPath + "\\MoulaCalc.db";

            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
        }

        public string createDbConnection()
        {
            string strCon = string.Format("Data Source={0}", dbFilePath);
            dbConnection = new SQLiteConnection(strCon);
            dbConnection.Open();
            command = dbConnection.CreateCommand();
            return strCon;
        }

        public SQLiteConnection Connection()
        {
            string strCon = string.Format("Data Source={0}", dbFilePath);
            dbConnection = new SQLiteConnection(strCon);
            dbConnection.Open();
            command = dbConnection.CreateCommand();
            return dbConnection;
        }

        public void closeDbConnection() {
            dbConnection.Close();
        }

        public void createTables()
        {
            if (!checkIfExist("AlloBank"))
            {
                sqlCommand = "CREATE TABLE AlloBank("
                             + "Id INTEGER PRIMARY KEY AUTOINCREMENT,"
                             + "Total NVARCHAR(100) NOT NULL,"
                             + "Date DATETIME"
                             + ");";
                executeQuery(sqlCommand);
            }
            if (!checkIfExist("Encours"))
            {
                sqlCommand = "CREATE TABLE Encours("
                             + "Id INTEGER PRIMARY KEY,"
                             + "Billet5 INTEGER,"
                             + "Billet10 INTEGER,"
                             + "Billet20 INTEGER,"
                             + "Billet50 INTEGER,"
                             + "Billet100 INTEGER,"
                             + "Billet200 INTEGER,"
                             + "Billet500 INTEGER"
                             + ");";
                executeQuery(sqlCommand);
            }
        }

        public bool checkIfExist(string tableName)
        {
            command.CommandText = "SELECT name FROM sqlite_master WHERE name='" + tableName + "'";
            var result = command.ExecuteScalar();

            return result != null && result.ToString() == tableName ? true : false;
        }

        public void executeQuery(string sqlCommand)
        {
            SQLiteCommand triggerCommand = dbConnection.CreateCommand();
            triggerCommand.CommandText = sqlCommand;
            triggerCommand.ExecuteNonQuery();
        }

        public bool checkIfTableContainsData(string tableName)
        {
            command.CommandText = "SELECT count(*) FROM " + tableName;
            var result = command.ExecuteScalar();

            return Convert.ToInt32(result) > 0 ? true : false;
        }

        public SQLiteDataReader executeSelectQuery(string select)
        {
            SQLiteCommand triggerCommand = dbConnection.CreateCommand();
            triggerCommand.CommandText = select;
            SQLiteDataReader reader = triggerCommand.ExecuteReader();
            return reader;
        }

        /*public void fillTable()
        {
            if (!checkIfTableContainsData("MY_TABLE"))
            {
                sqlCommand = "insert into MY_TABLE (code_test_type) values (999)";
                executeQuery(sqlCommand);
            }
        }*/
    }
}
