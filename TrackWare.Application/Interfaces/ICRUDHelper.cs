using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Domain.Entities;

namespace TrackWare.Application.Interfaces
{
    public interface ICRUDHelper
    {
        Task<CrudDataResponseDto> ResolveAsync(CrudDataRequestDto request);
 

        Task<SaveResultDTO> SaveDataAsync(bool createNew, string jsonData,string loginID);

        Task<CrudSettings> ResolveTypeSettingsAysnc(  string typeCode, string loginID);
    }
}
