using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Application.DTOs
{
    public class LoginRequestDto
    {
        public string TypeCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
