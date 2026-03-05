using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Domain.Entities;

namespace TrackWare.Application.Interfaces
{
    public interface ICrudDataRepository
    {
        Task<object?> GetByIdAsync(CrudSettings tc,CrudDataRequestDto req);
        Task<object?> GetEmptyAsync( CrudSettings tc );
    }
}
