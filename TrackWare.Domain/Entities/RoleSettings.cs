using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Domain.Entities
{
    public class RoleSettings
    {
        public string TypeCode { get; set; }           // ROLE_TYPE_CODE
        public string Id { get; set; }                 // ROLE_ID
        public string Description { get; set; }        // ROLE_DESCRIPTION

        public DateTime? CreateTs { get; set; }        // ROLE_CREATE_TS
        public string CreateBy { get; set; }           // ROLE_CREATE_BY

        public int? ApprovalStatus { get; set; }       // ROLE_APPROVAL_STATUS
        public int? AddStatus { get; set; }            // ROLE_ADD_STATUS
        public int? EditStatus { get; set; }           // ROLE_EDIT_STATUS
        public int? DeleteStatus { get; set; }         // ROLE_DELETE_STATUS
        public int? PrintStatus { get; set; }          // ROLE_PRINT_STATUS
        public int? ApproveStatus { get; set; }        // ROLE_APPROVE_STATUS

        public bool? AllowLayout { get; set; }         // ROLE_ALLOW_LAYOUT
        public bool? AllowCancelPosting { get; set; }  // ROLE_ALLOW_CANCEL_POSTING
        public bool? ViewSelfCreatedOnly { get; set; } // ROLE_VIEW_SELF_CREATED_ONLY
    }
}
