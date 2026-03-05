using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Infrastructure.Classes
{
    public class ColumnInfo
    {
        public string ColumnName { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsComputed { get; set; }
    }

}
