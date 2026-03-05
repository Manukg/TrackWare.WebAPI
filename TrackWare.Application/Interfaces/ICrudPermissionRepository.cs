using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Domain.Entities;

namespace TrackWare.Application.Interfaces
{
    public interface ICrudPermissionRepository
    {
        Task<CrudSettings> GetCrudMetaAsync(string typeCode,string loginID);
    }
}
