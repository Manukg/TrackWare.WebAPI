using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.Interfaces;
using TrackWare.Domain.Entities;

namespace TrackWare.Infrastructure.Repositories
{
    public class CompanyProfileRepository: ICompanyProfileRepositor
    {
        private readonly IDbConnection _dbConnection;


        public CompanyProfileRepository(IDbConnection dbConnection)
        {
            Console.WriteLine(dbConnection.ConnectionString);
            _dbConnection = dbConnection;
        }

        public async Task<List<CompanyProfile>> GetCompanyProfileList()
        {
            const string sql = @"SELECT 
                                    CMPPROF_TYPE_CODE   AS TypeCode,
                                    CMPPROF_ID         AS Id,
                                    CMPPROF_NAME       AS Name, 
                                    CMPPROF_EMAIL_ADDRESS AS EmailAddress,
                                    CMPPROF_MOBILE_NUMBER AS MobileNumber,
                                    CMPPROF_ADDRESS    AS Address,
                                    CMPPROF_CREATE_BY   AS CreatedBy,
                                    CMPPROF_CREATE_TS   AS CreateTs
                                FROM dbo.COMPANY_PROFILE_MASTER;";
          //  _dbConnection.Open();
            var result = await   _dbConnection.QueryAsync<CompanyProfile>(sql);
      //   await   Task.Delay(100);
            return result.ToList();
        }
        //public async Task<IEnumerable<CompanyProfile>> GetCompanyProfileList()
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        string sql = "SELECT Id, Name FROM CompanyProfile";

        //        var result = await connection.QueryAsync<CompanyProfile>(sql);
        //        return result;
        //    }
        //}

    }
}
