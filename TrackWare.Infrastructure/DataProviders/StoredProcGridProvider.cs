using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;

namespace TrackWare.Infrastructure.DataProviders
{
    public class StoredProcGridProvider : IGridDataProvider
    {
        //public async Task<GridResult> GetDataAsync(GridQuery query)
        //{
        //    // Call SP via Dapper
        //    return await _db.QueryAsync<GridResult>(
        //        "usp_" + query.TypeCode + "_List",
        //        new { query.Filters },
        //        commandType: CommandType.StoredProcedure
        //    );
        //}

        private readonly IDbConnection _dbConnection;

        public StoredProcGridProvider(IDbConnection dbConnection)
        {
            Console.WriteLine(dbConnection.ConnectionString);
            _dbConnection = dbConnection;
        }

        public async Task<object> GetDataAsync(ListParamDTO query)
        {

             var jsonParams = new
    {
        nMaxRows = query.MaxRowCount,
        strFmDate =  query.FromDate, 
        strToDate =   query.ToDate,
        strProcedureName = query.UspName,
        intApplyDateFilter=query.ApplyDateFilter
             };

    string jsonString = JsonSerializer.Serialize(jsonParams);

            var result = await _dbConnection.QueryAsync(
             "usp_sys_Execute_ListUsp",
             new
             {
                 strCMPCode = query.CMPCode,
                 strYearCode = query.YearCode,
                 strTypeCode = query.TypeCode,
                 strLoginId = query.loginID,
                 strJsonParams = jsonString
             },
             commandType: CommandType.StoredProcedure
         );
            return result;
        }
    }
}
