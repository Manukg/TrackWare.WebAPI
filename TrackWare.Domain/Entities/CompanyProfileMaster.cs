using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Domain.Entities
{
    public class CompanyProfile
    {
        public string TypeCode { get; set; }          // nvarchar(6), Primary Key
        public int Id { get; set; }                   // int, Primary Key

        public string Name { get; set; }              // nvarchar(40), Not Null
        public byte[]? Logo { get; set; }             // varbinary(max), Nullable
        public string EmailAddress { get; set; }      // nvarchar(max), Not Null
        public string? MobileNumber { get; set; }     // nvarchar(max), Nullable

        public string? Address { get; set; }          // nvarchar(500), Nullable
        public string? CreatedBy { get; set; }        // nvarchar(max), Nullable
        public DateTime CreateTs { get; set; }        // datetime, Not Null
    }
}
