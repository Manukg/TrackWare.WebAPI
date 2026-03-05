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
    public class MenuRepository:IMenuRepository
    {

        private readonly IDbConnection _dbConnection;

        public MenuRepository(IDbConnection dbConnection)
        {
            Console.WriteLine(dbConnection.ConnectionString);
            _dbConnection = dbConnection;
        }

        public async Task<List<MenuItem>?> GetMenu(string userType, string loginID)
        {
            // Write Code to call prodedure 
            var result = await this._dbConnection.QueryAsync<MenuItem>(
           "usp_sys_menuoptions",
           new { strUserType = userType, strLoginID = loginID },
           commandType: CommandType.StoredProcedure
       );

            return result.ToList<MenuItem>();


        }
    }
}
