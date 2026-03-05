using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Domain.Entities;

namespace TrackWare.Application.DTOs
{
    public class CrudDataResponseDto
    {
        public object? FormData { get; set; }
        public Dictionary<string, List<LookupDto>> Lookups { get; set; } = new();
      //public CrudSettings Meta { get; set; }
    }

}
