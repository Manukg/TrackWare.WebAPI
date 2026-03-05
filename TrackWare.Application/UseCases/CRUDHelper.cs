using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using TrackWare.Domain.Entities;

namespace TrackWare.Application.UseCases
{
    public class CRUDHelper:ICRUDHelper  
    {
        private readonly ICrudDataResolver _dataResolver;
        private readonly ICrudDataSaver _dataSaver;
        private readonly ICrudPermissionRepository _permission;
        public CRUDHelper(ICrudDataResolver dataResolver, ICrudDataSaver dataSaver, ICrudPermissionRepository permission)
        {
            this._dataResolver = dataResolver;
            this._dataSaver = dataSaver;
            this._permission = permission;
        }
        public async Task<CrudDataResponseDto> ResolveAsync(CrudDataRequestDto request)
        {            
            return await _dataResolver.ResolveAsync(request);
        }

        public async Task<CrudSettings> ResolveTypeSettingsAysnc(string typeCode, string loginID)
        {
          return await this._permission.GetCrudMetaAsync(typeCode, loginID);
        }

        public async Task<SaveResultDTO> SaveDataAsync(bool createNew, string jsonData, string loginID)
        {
            return await this._dataSaver.SaveDataAsync(createNew, jsonData,loginID);
        }
    }
}
