using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;

namespace TrackWare.Infrastructure.Repositories
{
    public class CRUDDataResolver:ICrudDataResolver
    {
        private readonly ICrudPermissionRepository _permissionRepo; 
        private readonly ICrudDataRepository _crudRepo;
        private readonly ILookupRepository _lookupRepo;

        public CRUDDataResolver(
            ICrudPermissionRepository permissionRepo,       
            ICrudDataRepository crudRepo,
            ILookupRepository lookupRepo)
        {
            _permissionRepo = permissionRepo; 
            _crudRepo = crudRepo;
            _lookupRepo = lookupRepo;
        }

        public async Task<CrudDataResponseDto> ResolveAsync(CrudDataRequestDto request)
        {
            var response = new CrudDataResponseDto();

            var settings = await _permissionRepo.GetCrudMetaAsync(request.TypeCode, request.LoginID);
            if(request.IsNewMode)
            {
                response.FormData = await _crudRepo.GetEmptyAsync(settings);
            }
            else
            {
                response.FormData = await _crudRepo.GetByIdAsync(settings, request);
            }
           

             
            foreach (var combo in request.ComboTypes)
            {
                response.Lookups[combo] = await _lookupRepo.LoadLookupAsync(combo, request.LoginID);
            }
           
            return response;
        }

         
    }
}
