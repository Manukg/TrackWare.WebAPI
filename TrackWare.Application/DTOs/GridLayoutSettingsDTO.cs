using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Domain.Entities;

namespace TrackWare.Application.DTOs
{
    public class GridLayoutSettingsDTO
    {
        public string TypeCode { get; set; }

        public string Context { get; set; }

        public string ProcedureName { get; set; }
        public List<GridColumnInfo> GridColumnInfos { get; set; }

        public string MoreSettings { get; set; }

        public string ApplicableRoles { get; set; }

        public string? LoginID { get; set; }

        public List<RoleSettings>? Roles { get; set; }

    }
}
