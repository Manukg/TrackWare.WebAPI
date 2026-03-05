using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using TrackWare.Domain.Entities;
 

namespace TrackWare.Infrastructure.Repositories
{
    public class CrudPermissionRepository : ICrudPermissionRepository
    {
        private IDbConnection _db;
        public CrudPermissionRepository(IDbConnection db)
        {
            this._db = db;
        }
        public async Task<CrudSettings> GetCrudMetaAsync(string typeCode, string loginID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@strLoginID", loginID);
       
            parameters.Add("@strTypeCode", typeCode);

            return await _db.QuerySingleAsync<CrudSettings>(
                "usp_sys_GetCrudPermissions",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        
    }
}
