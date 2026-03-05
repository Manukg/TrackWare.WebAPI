using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using TrackWare.Domain.Entities;
using Dapper;

namespace TrackWare.Infrastructure.Repositories
{
    public class ListOptionsRepository : IListOptionsRepository
    {
        private readonly IDbConnection _dbConnection;

        public ListOptionsRepository(IDbConnection dbConnection)
        {
            Console.WriteLine(dbConnection.ConnectionString);
            _dbConnection = dbConnection;
        }
        public async Task<ListOptionsDTO> GetSettings(string typeCode,string loginID)
        {
            var result = await this._dbConnection.QueryAsync<ListOptionsDTO>(
        "usp_sys_listoptions",
        new { strtypeCode = typeCode, strLoginID = loginID },
        commandType: CommandType.StoredProcedure
    );

            return result.First<ListOptionsDTO>();

        }

        public async Task<List<ListUspDTO >> GetLists(string typeCode, string loginID)
        {
            var result = await this._dbConnection.QueryAsync<ListUspDTO>(
        "usp_sys_tc_lists",
        new { strtypeCode = typeCode, strLoginID = loginID },
        commandType: CommandType.StoredProcedure
    );

            return result.ToList<ListUspDTO>();

        }
    }
}
