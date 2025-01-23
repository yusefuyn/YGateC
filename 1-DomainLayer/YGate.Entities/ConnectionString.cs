using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities
{
    public class ConnectionString
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
        public DbType Type { get; set; }
    }

    public enum DbType
    {
        Mysql,
        Postgresql,
        SqlLite,
        MsSql,
    }
}
