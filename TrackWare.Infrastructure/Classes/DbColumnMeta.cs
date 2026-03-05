using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Infrastructure.Classes
{
    public class DbColumnMeta
    {
        public string ColumnName { get; set; }
        public int? MaxLength { get; set; }     // -1 for MAX
        public string DataType { get; set; }
    }

}
