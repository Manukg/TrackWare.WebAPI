using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using Dapper;
using TrackWare.Application.Interfaces;

namespace TrackWare.Infrastructure.Repositories
{
    public class LookupRepository:ILookupRepository
    {
        private IDbConnection _db;
        public LookupRepository(IDbConnection db)
        {
            this._db = db;
        }
        public async Task<List<LookupDto>> LoadLookupAsync(string typeCode,string loginID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@strLoginID", loginID);
            parameters.Add("@strTypeCode", typeCode);

            var result = await _db.QueryAsync<LookupDto>(
                "usp_sys_GetLookUpData",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

    }
}
