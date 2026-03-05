using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Infrastructure.Classes
{
    public class LengthViolation
    {
        public string ColumnName { get; set; }
        public int MaxLength { get; set; }
        public int ActualLength { get; set; }
    }
}
