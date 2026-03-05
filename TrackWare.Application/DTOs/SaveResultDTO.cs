using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.Enum;

namespace TrackWare.Application.DTOs
{
    public class SaveResultDTO
    {
        public SaveActionStatus SaveActionStatus { get; set; }
        public string Message { get; set; }

        public string NewID { get; set; }
    }
}
