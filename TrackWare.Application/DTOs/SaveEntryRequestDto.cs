using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Application.DTOs
{
    public class SaveEntryRequestDto
    {
        public bool IsNew { get; set; }
        public string TypeCode { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }

}
