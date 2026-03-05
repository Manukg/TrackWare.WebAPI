using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Application.DTOs
{
    public class LoginResponseDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string EmailID { get; set; }

        public string Mobile { get; set; }

        public string CMPCode { get; set; }

        public string YearCode { get; set; }


     

        public byte[] UserPhoto { get; set; }

        public string Role { get; set; }

        public string LicCompany { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; } // Optional JWT token
    }
}
