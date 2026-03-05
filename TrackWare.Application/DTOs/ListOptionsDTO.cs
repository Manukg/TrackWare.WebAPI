using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Application.DTOs
{
    public class ListOptionsDTO
    {
        public string TypeCode { get; set; }
        public  int MaxRowCount { get; set; }
        public string TypeName { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowRemove { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowApprove { get; set; }

        public bool AllowLayoutChange { get; set; }

        public string DefaultQuery { get; set; }

        public string DefaultFilter { get; set; }

        public string ModuleName { get; set; }

        public List<ListUspDTO> ListProc { get; set; }


        public List<PrintOptionsDTO> PrintOptions { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string UserCSV { get; set; }

    }
}
    

