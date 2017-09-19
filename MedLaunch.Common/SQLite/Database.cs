using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Common.SQLite
{
    public class Database
    {
        // default constructor
        public Database()
        {
            List<Tab> Tables = new List<Tab>();
        }

        // properties
        public List<Tab> Tables { get; set; }
    }

    public class Tab
    {
        // constructor
        public Tab()
        {
            List<Col> Columns = new List<Col>();
        }
        // properties
        public string TableName { get; set; }
        public List<Col> Columns { get; set; }
        public List<Data> Data { get; set; }
        public string PrimaryKeyColumn { get; set; }
    }

    public class Col
    {
        // constructor
        public Col()
        {

        }
        // properties
        public string ColName { get; set; }
        public string ColType { get; set; }
    }

    public class Data
    {
        // properties
        public string TableName { get; set; }
        public string ColName { get; set; }
        public string ColType { get; set; }
        public int PrimaryKeyValue { get; set; }
        public string Value { get; set; }
    }
}
