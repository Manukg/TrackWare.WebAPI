using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }           // User ID
        public string LoginId { get; set; }     // Login username
        public string LoginPassword { get; set; } // Hashed password

        public string EditPassword { get; set; }
        public string FullName { get; set; } // nvarchar(40) - Required

        public byte[]? Photo { get; set; } // varbinary(MAX) - Optional

        public string EmailAddress { get; set; } // nvarchar(MAX) - Required

        public string RoleID { get; set; } // nvarchar(MAX) - Required

        public string? MobileNumber { get; set; } // nvarchar(MAX) - Optional

        public string? Gender { get; set; } // nvarchar(50) - Optional

        public string? Address { get; set; } // nvarchar(500) - Optional
        public string CreateBy { get; set; }
        public DateTime CreateTs { get; set; }
    }
}
