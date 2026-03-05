using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Domain.Entities;

namespace TrackWare.Application.Interfaces
{
    public interface IRoleRepository
    {
      Task<RoleSettings?> GetRoleSettings(string typeCode, string roleId);
        Task<List<RoleSettings>> GetAllRoleSettings(string typeCode);

    }
}
