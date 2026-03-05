using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Application.DTOs
{
    public class MenuItemDTO
    {
        public string Label { get; set; }
        public string Icon { get; set; }
        public bool IsCollapsed { get; set; }
        public bool IsTitle { get; set; }
        public string Url { get; set; }
       
        public string QueryString { get; set; }

        public List<MenuItemDTO> Children { get; set; }
    }
}
