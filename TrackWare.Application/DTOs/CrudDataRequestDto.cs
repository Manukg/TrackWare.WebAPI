using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Application.DTOs
{
    public class CrudDataRequestDto
    {
        public string TypeCode { get; set; }     // TC
        public string? Id { get; set; }             // Primary key value
        public List<string> ComboTypes { get; set; } = new();
        public string? LoginID { get; set; }

        public string? CMPCode { get; set; }
        public string? YearCode { get; set; }

        public bool IsNewMode { get; set; }
        // Primary key value

    }
}
