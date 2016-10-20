using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Data.Entity;

namespace Asnitech.SQLite
{
    public class Operations
    {
        // default constructor
        public Operations()
        {

        }

        public static bool CheckDbExists(string dbPath)
        {            
            // first check whether the database exists - return if it does not
            if (!File.Exists(dbPath))
                return false;
            else { return true; }
        }

        public static string GetDbVersion()
        {
            string dbPath = @"Data\Settings\MedLaunch.db";
            // create System.Data.SQLite connection
            string connString = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + dbPath + "; Pooling=False; Read Only=True;";

            string dbVersion = "";
            // connect to database and retreive the current version
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                StringBuilder query = new StringBuilder();
                query.Append("SELECT dbVersion ");
                query.Append("FROM Versions ");
                query.Append("WHERE versionId = 1");
                using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                {
                    conn.Open();
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //Console.WriteLine(dr.GetValue(0) + " " + dr.GetValue(1) + " " + dr.GetValue(2));
                            dbVersion = dr.GetValue(0).ToString();
                        }
                    }
                    conn.Close();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            SQLiteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return dbVersion;
        }

        public static void RestoreDatabaseData(Database db)
        {
            // get current database


            // iterate through each table
            foreach (Tab table in db.Tables)
            {
                string tableName = table.TableName;
                string primKeyName = table.PrimaryKeyColumn;

                // get all primary key values into an array (this will form the rows)
                int[] keys = (from a in table.Data
                              select a.PrimaryKeyValue).ToArray();

                List<List<Data>> rows = new List<List<Data>>();
                // build a list of rows
                int i = 0;
                while (i < keys.Length)
                {
                    List<Data> d = new List<Data>();
                    // get all data objects that have a primary key of value keys[i]
                    d = (from a in table.Data
                        where a.PrimaryKeyValue == keys[i]
                        select a).ToList();
                    // add list to the master list
                    rows.Add(d);
                    i++;
                }
                
                // we should now have a List that contains Lists of row fields (each one being of the same row).      
                // iterate through top level list
                foreach (List<Data> list in rows)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + @"Data\Settings\MedLaunch.db"))
                    {

                   
                        // at this level we have a List (row) containing every field on that row
                        foreach (Data r in list)
                        {
                            // at this level we are iterating through all the fields in a single row
                        }
                    }
                }
            }
        }

        

        public static Database GetDatabaseObject(string dbPath)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            List<DataTable> dts = new List<DataTable>();
            Database db = new Database();
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = @"Data Source=" + dbPath;
                connection.Open();
                DataTable dttables = connection.GetSchema("Tables");
                DataTable dtcolumns = connection.GetSchema("Columns");
                DataTable dtdatatypes = connection.GetSchema("DataTypes");

                dts.Add(dttables);
                dts.Add(dtcolumns);
                dts.Add(dtdatatypes);

                // build up class from the database
                
                List<Tab> tables = new List<Tab>();

                // get all tables    
                var t = from a in dttables.AsEnumerable()
                        select a;
                var b = from a in t
                        select a.ItemArray;
                // get all columns
                var y = from a in dtcolumns.AsEnumerable()
                        select a;
                var x = from a in y
                        select a.ItemArray;

                // iterate through each table and build up the db object
                foreach (var obj in b)
                {
                    // first iterate through every table and create a table object
                    // [3] = table type   [2] = table name

                    if (obj[3].ToString() != "table")
                    {
                        // this is maybe a system table - skip it
                        continue;
                    }
                    Tab table = new Tab();
                    table.TableName = obj[2].ToString();
                    List<Col> columns = new List<Col>();
                    List<Data> datas = new List<Data>();

                    // now get each column for this particular table
                    foreach (var co in x.Where(g => g[2].ToString() == table.TableName))
                    {
                        // [2] - tablename   [3] - column name    [11] - type
                        Col column = new Col();
                        column.ColName = co[3].ToString();
                        column.ColType = co[11].ToString();
                        columns.Add(column);
                    }

                    table.Columns = columns;

                    using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=" + dbPath + "; Pooling=False; Read Only=True;"))
                    {                        
                        StringBuilder query = new StringBuilder();
                        // build select query with our known columns
                        query.Append("SELECT ");
                        string[] cNames = (from a in table.Columns
                                 select a.ColName).ToArray();

                        int cNamesCount = cNames.Length;
                        // set primary key field
                        table.PrimaryKeyColumn = cNames[0];
                        int i = 0;
                        while (i < cNamesCount)
                        {
                            query.Append(cNames[i]);
                            if (i != cNamesCount - 1)
                                query.Append(", ");
                            query.Append(" ");
                            i++;
                        }                       
                        
                        query.Append("FROM " + table.TableName + " ");                     

                        List<Data> DataList = new List<Data>();

                        using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                        {
                            conn.Open();
                            using (SQLiteDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    int cnt = 0;
                                    // loop through each returned cell
                                    while (cnt < cNamesCount)
                                    {
                                        Data data = new Data();
                                        data.PrimaryKeyValue = Convert.ToInt32(dr.GetValue(0));
                                        data.TableName = table.TableName;
                                        data.ColName = cNames[cnt];
                                        data.Value = dr.GetValue(cnt).ToString();
                                        // get the column type
                                        data.ColType = (from u in table.Columns
                                                        where u.ColName == cNames[cnt]
                                                        select u.ColType).FirstOrDefault();
                                        DataList.Add(data);
                                        cnt++;
                                    }
                                }
                            }
                            conn.Close();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        table.Data = DataList;   
                                             
                    }
                    SQLiteConnection.ClearAllPools();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    tables.Add(table);
                }
                db.Tables = tables;                          
            }
            return db;
        }       
    }   
}
