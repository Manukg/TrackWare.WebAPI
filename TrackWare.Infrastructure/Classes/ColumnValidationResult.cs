using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Infrastructure.Classes
{
    public class ColumnValidationResult
    {
        public List<string> InvalidColumns { get; set; } = new();
        public List<LengthViolation> LengthViolations { get; set; } = new();
    }
}
