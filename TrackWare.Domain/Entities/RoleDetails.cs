using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Domain.Entities
{
    public class RoleDetails
    {
        public string TypeCode { get; set; }              // ROLEDET_TYPECODE
        public string Id { get; set; }                    // ROLEDET_ID
        public int SlNo { get; set; }                     // ROLEDET_SLNO

        public string OptionType { get; set; }            // ROLEDET_OPTION_TYPE (default = 'MENU')
        public string OptionId { get; set; }              // ROLEDET_OPTION_ID

        public string Text { get; set; }                  // ROLEDET_TEXT
        public bool? ShowInQuickAccess { get; set; }      // ROLEDET_SHOW_IN_QUICK_ACCESS (default = 0)
    }
}
