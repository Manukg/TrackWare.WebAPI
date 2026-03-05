using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Application.DTOs
{
    public class ListParamDTO
    {
        public string CMPCode { get; set; }
        public string YearCode { get; set; }
        public string TypeCode { get; set; }
       
       public string UspName { get; set; }

        public bool ApplyDateFilter { get; set; }

        public int MaxRowCount { get; set; }
        public string? FromDate { get; set; }

        public string? ToDate { get; set; }

        public string loginID { get; set; }

       
    }
}
