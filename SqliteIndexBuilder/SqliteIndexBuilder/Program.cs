using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqliteIndexBuilder.Tokenizer;

namespace SqliteIndexBuilder
{
    class Program
    {
        private static string datasource = AppDomain.CurrentDomain.BaseDirectory + "index.db";
        static void Main(string[] args)
        {
            CreateDataBase();
            BuildIndex();
            GetData("SELECT  docid, title FROM pages WHERE pages MATCH 'renminbi'");
            GetData("SELECT  docid, title FROM pages WHERE pages MATCH 'イズム'");
            GetData("SELECT  docid, title FROM pages WHERE pages MATCH 'イズムを持つ必要*'");
            Console.ReadKey();
        }

        private static void CreateDataBase()
        {
            if (File.Exists(datasource))
            {
                File.Delete(datasource);
            }

            SQLiteConnection conn = GetConnection();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = "CREATE VIRTUAL TABLE pages USING fts3(title, body, tokenize=cjk,simple,porter)";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private static SQLiteConnection GetConnection()
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

        private static void BuildIndex()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\files_utf8\\";
            string[] files = Directory.GetFiles(path);
            int i = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            foreach (string p in files)
            {
                i++;
                string content = File.ReadAllText(p);
                FileInfo finfo = new FileInfo(p);
                string name = finfo.Name;
                string sql = "INSERT INTO pages (docid, title, body) VALUES (@docid, @name, @content);";
                List<SQLiteParameter> list = new List<SQLiteParameter>();
                SQLiteParameter pId = new SQLiteParameter("@docid");
                pId.Value = i;
                SQLiteParameter pName = new SQLiteParameter("@name");
                pName.Value = name;
                SQLiteParameter pContent = new SQLiteParameter("@content");
                pContent.Value = content;
                list.Add(pId);
                list.Add(pName);
                list.Add(pContent);
                Console.WriteLine(i + ":" + name + ":" + finfo.Length);
                ExecuteSql(sql, list);

            }
            sw.Stop();
            Console.WriteLine((sw.ElapsedMilliseconds / 1000));
        }

        private static int ExecuteSql(string sql)
        {
            SQLiteConnection conn = GetConnection();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = sql;

            int returnVal = cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            return returnVal;
        }

        private static int ExecuteSql(string sql, List<SQLiteParameter> parameters)
        {
            SQLiteConnection conn = GetConnection();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = sql;
            foreach (SQLiteParameter p in parameters)
            {
                cmd.Parameters.Add(p);
            }
            int returnVal = cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
            return returnVal;
        }

        private static void GetData(string sql)
        {
            SQLiteConnection conn = GetConnection();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = sql;
            SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Console.WriteLine(string.Format("{0}\t{1}\t", dr[0], dr[1]));
                }
            }
            Console.WriteLine("------------------------------------------");
            conn.Close();
        }
    }
}
