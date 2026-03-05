using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Domain.Entities
{
    public class MenuItem
    {
        public string ParentID { get; set; }        
        public string Order { get; set; }          
        public int IsTitle { get; set; }            
        public string MenuCaption { get; set; }
        public string Url { get; set; }
        public string Arg { get; set; }            
        public string IconComponentName { get; set; }

    }
}
