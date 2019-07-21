using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCheat
{
    class SQLiteDB
    {
        private static string dbFileName = "db.sqlite";
        private static SQLiteConnection m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
        private static SQLiteCommand m_sqlCmd = new SQLiteCommand();
        public static void Connect()
        {
            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;
        }

        public static void Disconnect()
        {
            m_dbConn.Close();
        }

        public static void ExecQuery(string sql)
        {
            m_sqlCmd.CommandText = sql;
            m_sqlCmd.ExecuteNonQuery();
        }

        public static DataTable Select(string sql)
        {
            DataTable dTable = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, m_dbConn);
            adapter.Fill(dTable);
            return dTable;
        }
    }
}
