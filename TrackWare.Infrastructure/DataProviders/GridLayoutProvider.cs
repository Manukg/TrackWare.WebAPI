using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
 
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using static Dapper.SqlMapper;

namespace TrackWare.Infrastructure.DataProviders
{
    public class GridLayoutProvider : IColumnSchemaProvider
    {
        private readonly IDbConnection _dbConnection;
        private readonly IRoleRepository _roleRepository;

        public GridLayoutProvider(IDbConnection dbConnection, IRoleRepository roleRepository)
        {
            Console.WriteLine(dbConnection.ConnectionString);
            _dbConnection = dbConnection;
            _roleRepository = roleRepository;
        }

        public async Task<GridLayoutSettingsDTO> GetColumnAsync(GridLayoutArgDTO arg)
        {
            List<GridColumnInfo> columnToReturn = new List<GridColumnInfo>();
            string moreSettings = string.Empty;
            string columnDefinition = string.Empty;

            
                var settings = await _dbConnection.QueryAsync(
           "usp_sys_get_list_layout",
           new
           {
              
               strTypeCode = arg.TypeCode,
               strLoginId = arg.loginID,
               strProcedureName = arg.ListProcedure,
           },
           commandType: CommandType.StoredProcedure
       );
                var setting = settings.FirstOrDefault();
            if (setting != null)
            {
                moreSettings = setting.MoreSettings;
                columnDefinition = setting.ColumnDefinition;
               
                if (!string.IsNullOrEmpty(columnDefinition))
                {
                    columnToReturn = JsonSerializer.Deserialize<List<GridColumnInfo>>(columnDefinition);
                }

            }
       
            return new GridLayoutSettingsDTO { GridColumnInfos = columnToReturn,MoreSettings= moreSettings } ;
        }

        public async Task SaveLayout(GridLayoutSettingsDTO arg)
        {
            //throw new NotImplementedException();
            string jsonColumns = JsonSerializer.Serialize(arg.GridColumnInfos);

            string sql = @"
        INSERT INTO GRID_LAYOUT_HEADER
        (GL_TYPE, GL_CONTEXT, GL_PROC_NAME, GL_EFFDATE, GL_APP_ROLES, GL_REMARKS, GL_JSON_COL_SETTINGS, GL_CREATE_BY, GL_SETTINGS)
        VALUES
        (@GL_TYPE, @GL_CONTEXT, @GL_PROC_NAME, GETDATE(), @GL_APP_ROLES, @GL_REMARKS, @GL_JSON_COL_SETTINGS, @GL_CREATE_BY, @GL_SETTINGS);

        SELECT SCOPE_IDENTITY();
    ";

          
                var newId = await _dbConnection.ExecuteScalarAsync<int>(sql, new
                {
                    GL_TYPE = arg.TypeCode,
                    GL_CONTEXT = arg.Context,
                    GL_PROC_NAME = arg.ProcedureName,
                    GL_APP_ROLES = arg.ApplicableRoles,
                    GL_REMARKS = "",               // optional
                    GL_JSON_COL_SETTINGS = jsonColumns,
                    GL_SETTINGS = arg.MoreSettings,
                    GL_CREATE_BY = arg.LoginID
                    // saving here
                });

             //   return $"Saved Successfully. ID = {newId}";
              

        }

        public async Task<GridLayoutSettingsDTO> ShowAvailableColumns(GridLayoutArgDTO arg)
        {
            List<GridColumnInfo> actualGridColumns = new List<GridColumnInfo>();
            string moreSettings = string.Empty;
            string applicableRoles = string.Empty;
            string savedSettings = string.Empty;

            var jsonParams = new
            {
                nMaxRows = 0,
                strFmDate = DateTime.Now.Date.ToString("yyyyMMdd"),
                strToDate = DateTime.Now.Date.ToString("yyyyMMdd"),
                strProcedureName = arg.ListProcedure,
                intApplyDateFilter = ""
            };

            string jsonString = JsonSerializer.Serialize(jsonParams);
            DataTable dt = new DataTable();

            using (var cmd = new SqlCommand("usp_sys_Execute_ListUsp", (SqlConnection)_dbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@strCMPCode", arg.CMPCode);
                cmd.Parameters.AddWithValue("@strYearCode", arg.YearCode);
                cmd.Parameters.AddWithValue("@strTypeCode", arg.TypeCode);
                cmd.Parameters.AddWithValue("@strLoginId", arg.loginID);
                cmd.Parameters.AddWithValue("@strJsonParams", jsonString);

                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                foreach (DataColumn column in dt.Columns)
                {
                    actualGridColumns.Add(new GridColumnInfo
                    {
                        Name = column.ColumnName,
                        DataType = column.DataType.Name
                    });
                }

                var settings = await _dbConnection.QueryAsync(
           "usp_sys_get_list_layout",
           new
           {
              
               strTypeCode = arg.TypeCode,
               strLoginId = arg.loginID,
               strProcedureName = arg.ListProcedure,
           },
           commandType: CommandType.StoredProcedure
       );
                var setting = settings.FirstOrDefault();
                if (setting != null)
                {
                    moreSettings = setting.MoreSettings;
                    applicableRoles = setting.ApplicableRoles;
                    savedSettings = setting.ColumnDefinition;
                }
            }
            List<GridColumnInfo> savedList=new List<GridColumnInfo>();
            if (!string.IsNullOrEmpty(savedSettings))
            {
                savedList = JsonSerializer.Deserialize<List<GridColumnInfo>>(savedSettings);
            }
            var merged = actualGridColumns
    .Select(col =>
    {
        var saved = savedList.FirstOrDefault(s => s.Name == col.Name);
        return saved ?? col;
    })
    .ToList();

            var roles = await this._roleRepository.GetAllRoleSettings(arg.RoleType);

            return new GridLayoutSettingsDTO { GridColumnInfos = merged, MoreSettings = moreSettings, ApplicableRoles = applicableRoles, Roles = roles.ToList() };
        }

        
    }
}
