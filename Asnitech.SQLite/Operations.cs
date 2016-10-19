using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;


namespace Asnitech.SQLite
{
    public class Operations
    {
        // default constructor
        public Operations()
        {

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

                    using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=" + dbPath))
                    {
                        
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * ");
                        query.Append("FROM " + table.TableName + " ");
                        conn.Open();
                        // now sql call to get all data for this table
                        var cmd = new SQLiteCommand(query.ToString(), conn);
                        var dr = cmd.ExecuteReader();
                        foreach (var thing in dr)
                        {
                         
                        }
                        for (var i = 0; i < dr.FieldCount; i++)
                        {
                            //Console.WriteLine(dr.GetName(i));
                            Data dat = new Data();

                        }
                        conn.Close();
                    }

                    tables.Add(table);
                }

                db.Tables = tables;                          
            }
            return db;
        }

        

    }

    
}
