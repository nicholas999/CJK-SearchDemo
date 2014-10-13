using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using CJKSearch.Tokenizer;

namespace CJKSearch.Helper
{
    public static class SQLiteHelper
    {
        const string datasource = @"C:\Users\wuxnx\Documents\Visual Studio 2012\Projects\CJKSearch\CJKSearch\bin\test.db";
        private static SQLiteConnection GetConn()
        {

            SQLiteConnection conn = new SQLiteConnection();
            SQLiteConnectionStringBuilder scsb = new SQLiteConnectionStringBuilder();
            scsb.DataSource = datasource;
            conn.ConnectionString = scsb.ToString();
            conn.Open();
            CJKTokenizer tokenizer = new CJKTokenizer();
            tokenizer.RegisterMe(conn);
            return conn;
        }

        public static DataSet GetData(string sql)
        {
            SQLiteConnection conn = GetConn();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = sql;
            SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            conn.Close();
            return ds;
        }
    }
}