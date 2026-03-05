using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Domain.Entities
{
  public  enum NumberingMethod
    {
        Idnentity,
        AutoNumber,
        ManualNumber 
    }
    public class CrudSettings
    {
        public string CCMPCode { get; set; }
        public string YearCode { get; set; }

        public string LoginID { get; set; }
        // ===== Identity =====
        public string TypeCode { get; set; }              // TC_TYPECODE
        public string Description { get; set; }           // TC_DESCRIPTION

        // ===== Permissions (resolved via CASE statements) =====
        public bool CanAdd { get; set; }                   // Computed
        public bool CanEdit { get; set; }                  // Computed
        public bool CanDelete { get; set; }                // Computed
        public bool CanCancelApprove { get; set; }         // Computed
        public bool CanApprove { get; set; }               // Computed
        public bool CanPrint { get; set; }                 // Computed

        // ===== Partial Views =====
        public string PartialView1 { get; set; }          // TC_PARTIAL_VIEW1
        public string PartialView2 { get; set; }          // TC_PARTIAL_VIEW2
        public string PartialView3 { get; set; }          // TC_PARTIAL_VIEW3
        public string PartialView4 { get; set; }          // TC_PARTIAL_VIEW4

        // ===== Table & Numbering =====
        public string TableName { get; set; }             // MOD_HEADER_TABLE
        public string Prefix { get; set; }                // MOD_HEADER_PREFIX
        public string NumberPrefix { get; set; }          // TC_NUMBER_PREFIX
        public string NumberSuffix { get; set; }          // TC_NUMBER_SUFFIX
     
        public NumberingMethod  NumberingMethod { get; set; }       // TC_NUMBERING_METHOD

        // ===== Relationships =====
        public string PreviousType { get; set; }          // TC_PREVIOUS_TYPECODE
        public string PreviousTypes { get; set; }         // TC_PREVIOUS_TYPECODELIST

        // ===== Extensions =====
        public string UDFields { get; set; }              // TC_UD_FIELDS
        public string ExtensionData { get; set; }         // TC_EXTENEDFIELD_DATA


        public bool IsCompanySpecific { get; set; }               // Computed
        public bool IsYearSpecific { get; set; }                 // Computed

        public string LoadAPI { get; set; }          // TC_PARTIAL_VIEW1

        public string SaverAPI { get; set; }

        public Dictionary<string, string> SubTables { get; set; }
    }

}
