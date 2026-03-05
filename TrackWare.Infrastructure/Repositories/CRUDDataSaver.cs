using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Application.Enum;
using TrackWare.Application.Interfaces;
using TrackWare.Domain.Entities;
using TrackWare.Infrastructure.Classes;
using static Dapper.SqlMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrackWare.Infrastructure.Repositories
{
    public class CRUDDataSaver : ICrudDataSaver
    {
        private readonly ICrudPermissionRepository _permissionRepo;
        private readonly IDbConnection _dbConnection;

        public CRUDDataSaver( ICrudPermissionRepository permissionRepo, IDbConnection dbConnection)
        {
            _permissionRepo = permissionRepo;
            _dbConnection = dbConnection;

        }
        public async Task<SaveResultDTO> SaveDataAsync(bool isNew, string jsonData,string loginID)
        {
            var returnValu = new SaveResultDTO();
            try
            {
                var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData);
                var typeCode = jsonDict["TypeCode"].ToString();
                var settings = await _permissionRepo.GetCrudMetaAsync(typeCode, loginID);

                if (isNew && !settings.CanAdd)
                    throw new UnauthorizedAccessException("Add permission denied");

                if (!isNew && !settings.CanEdit)
                    throw new UnauthorizedAccessException("Edit permission denied");

                var columns = await GetTableColumnsAsync(settings.TableName);


                var dbColumns = await GetDbColumnMetaAsync(settings.TableName);

                var validation = ValidateJsonColumns(jsonDict, settings, dbColumns);

                // 1️⃣ Invalid column names check
                if (validation.InvalidColumns.Any())
                {
                    return new SaveResultDTO
                    {
                        SaveActionStatus = SaveActionStatus.validationError,
                        Message = $"Invalid column(s): {string.Join(", ", validation.InvalidColumns)}",
                       
                    };
                }

                // 2️⃣ Length violation check
                if (validation.LengthViolations.Any())
                {
                    var lengthMessages = validation.LengthViolations
                        .Select(v => $"{v.ColumnName} (Max: {v.MaxLength}, Actual: {v.ActualLength})");

                    return new SaveResultDTO
                    {
                        SaveActionStatus = SaveActionStatus.validationError,
                        Message = $"Data length exceeded for column(s): {string.Join("; ", lengthMessages)}",
                
                    };
                }

                var (sql, parameters) = BuildSaveQuery(isNew, settings, jsonDict, columns);
              
                if (isNew && settings.NumberingMethod == NumberingMethod.Idnentity)
                {
                   
                    // Add logic for retrieving Identity value 
                    var newId = await _dbConnection.ExecuteScalarAsync<object>(sql, parameters);
                    returnValu.NewID = newId?.ToString();
                }
                else
                {
                    await _dbConnection.ExecuteAsync(sql, parameters);
                    returnValu.NewID = parameters.Get<object>($"@{CrudDataRepository.ToPascalAlias($"{settings.Prefix}_ID", settings.Prefix)}")?.ToString();
                    // To Do: Implement your saving logic here using typeSettings and jsonData
                }
                returnValu.SaveActionStatus = SaveActionStatus.Success;
              

            }
            catch (Exception ex)
            {
                returnValu.SaveActionStatus = SaveActionStatus.UnknownError;
                returnValu.Message = ex.Message;
                
            }
            return returnValu;
        }
        private (string sql, DynamicParameters parameters) BuildSaveQuery(
            bool isNew,
            CrudSettings settings,
            Dictionary<string, object> data,
            List<ColumnInfo> columns)
        {

            var createdTsDb = $"{settings.Prefix}_Create_TS";
            var createdByDb = $"{settings.Prefix}_Create_By";

            var createdTsJson = CrudDataRepository.ToPascalAlias(createdTsDb, settings.Prefix);
            var createdByJson = CrudDataRepository.ToPascalAlias(createdByDb, settings.Prefix);

            var parameters = new DynamicParameters();

            // 1️⃣ Build DB → JSON alias map
            var dbToJsonMap = columns
                .Where(c => !c.IsIdentity && !c.IsComputed)
                .ToDictionary(
                    c => CrudDataRepository.ToPascalAlias(c.ColumnName, settings.Prefix), // JSON property
                    c => c.ColumnName,                                 // DB column
                    StringComparer.OrdinalIgnoreCase
                );

         

            // 2️⃣ Filter JSON data (ignore _tmp fields)
            var filteredData = data
                .Where(d =>
                    !d.Key.EndsWith("_tmp", StringComparison.OrdinalIgnoreCase) &&
                    dbToJsonMap.ContainsKey(d.Key))
                .ToDictionary(
                    d => d.Key,                     // JSON property
                    d => d.Value
                );
            if (isNew)
            {
                filteredData[createdTsJson] = DateTime.UtcNow; // or DateTime.Now
                filteredData[createdByJson] = settings.LoginID;

                // Ensure mapping exists (even if fields not sent from UI)
                if (!dbToJsonMap.ContainsKey(createdTsJson))
                    dbToJsonMap[createdTsJson] = createdTsDb;

                if (!dbToJsonMap.ContainsKey(createdByJson))
                    dbToJsonMap[createdByJson] = createdByDb;
            }
            // 3️⃣ Add parameters using JSON property names
            foreach (var item in filteredData)
            {
                parameters.Add(
"@" + item.Key,
ConvertJsonElement(item.Value)
);
            }
 
            //   parameters.Add("@" + item.Key, item.Value);

            var pkDbColumn = $"{settings.Prefix}_ID";
            var typeCodeColumn = $"{settings.Prefix}_TYPE_CODE";
            var companyCodeColumn = $"{settings.Prefix}_CMPCODE";
            var yearCodeColumn = $"{settings.Prefix}_YEARCODE";
            var pkJsonProperty = CrudDataRepository.ToPascalAlias(pkDbColumn, settings.Prefix);

            if (isNew)
            {
                var columnList = string.Join(", ",
                    filteredData.Keys.Select(k => dbToJsonMap[k]));

                var paramList = string.Join(", ",
                    filteredData.Keys.Select(k => "@" + k));

                var insertSql = $"""
            INSERT INTO {settings.TableName} ({columnList})
            VALUES ({paramList});
        """;
                if (settings.NumberingMethod == NumberingMethod.Idnentity) {
                    insertSql = string.Concat(insertSql, ";", "SELECT CAST(SCOPE_IDENTITY() AS INT)");
                }
                
                return (insertSql, parameters);
            }
            else
            {
                if (!data.ContainsKey(pkJsonProperty))
                    throw new Exception($"Primary key {pkJsonProperty} not found in JSON");

                var setClause = string.Join(", ",
                    filteredData
                        .Where(d => !d.Key.Equals(pkJsonProperty, StringComparison.OrdinalIgnoreCase))
                        .Select(d => $"{dbToJsonMap[d.Key]} = @{d.Key}")
                );

                parameters.Add("@" + pkJsonProperty, ConvertJsonElement(data[pkJsonProperty]));

                // Prepare JSON parameter names and add type/company/year parameters
                var typeCodeJson = CrudDataRepository.ToPascalAlias(typeCodeColumn, settings.Prefix);
                parameters.Add("@" + typeCodeJson, settings.TypeCode);

                var whereConditions = new List<string>
                {
                    $"{pkDbColumn} = @{pkJsonProperty}",
                    $"{typeCodeColumn} = @{typeCodeJson}"
                };

                if (settings.IsCompanySpecific)
                {
                    var cmpJson = CrudDataRepository.ToPascalAlias(companyCodeColumn, settings.Prefix);
                    parameters.Add("@" + cmpJson, settings.CCMPCode);
                    whereConditions.Add($"{companyCodeColumn} = @{cmpJson}");
                }

                if (settings.IsYearSpecific)
                {
                    var yearJson = CrudDataRepository.ToPascalAlias(yearCodeColumn, settings.Prefix);
                    parameters.Add("@" + yearJson, settings.YearCode);
                    whereConditions.Add($"{yearCodeColumn} = @{yearJson}");
                }

                var whereClause = string.Join(" AND ", whereConditions);

                var updateSql = $"""
            UPDATE {settings.TableName}
            SET {setClause}
            WHERE {whereClause};
        """;

                return (updateSql, parameters);
            }
        }

        private async Task<List<DbColumnMeta>> GetDbColumnMetaAsync(string tableName)
        {
            var sql = """
        SELECT 
            COLUMN_NAME       AS ColumnName,
            CHARACTER_MAXIMUM_LENGTH AS MaxLength,
            DATA_TYPE         AS DataType
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME = @TableName
    """;

            return (await _dbConnection.QueryAsync<DbColumnMeta>(sql, new { TableName = tableName }))
                   .ToList();
        }
        private async Task<List<ColumnInfo>> GetTableColumnsAsync(string tableName)
        {
            var sql = """
        SELECT 
            c.name AS ColumnName,
            c.is_identity AS IsIdentity,
            c.is_computed AS IsComputed
        FROM sys.columns c
        INNER JOIN sys.tables t ON c.object_id = t.object_id
        WHERE t.name = @TableName
    """;

            return (await _dbConnection.QueryAsync<ColumnInfo>(sql, new { TableName = tableName }))
                   .ToList();
        }

        public ColumnValidationResult ValidateJsonColumns(
    Dictionary<string, object> jsonData,
    CrudSettings settings,
    List<DbColumnMeta> dbColumns)
        {
            var result = new ColumnValidationResult();

            var dbColumnLookup = dbColumns
                .ToDictionary(c => c.ColumnName, StringComparer.OrdinalIgnoreCase);

            foreach (var kv in jsonData)
            {
                // ⛔ Skip temporary columns (ends with _tmp)
                if (kv.Key.EndsWith("_tmp", StringComparison.OrdinalIgnoreCase))
                    continue;
                // Convert JSON key → DB column using your alias logic
                var dbColumnName = kv.Key.Contains("_")
                    ? kv.Key
                    : $"{settings.Prefix}_{ToDbColumnName(kv.Key)}";

                if (!dbColumnLookup.TryGetValue(dbColumnName, out var columnMeta))
                {
                    result.InvalidColumns.Add(dbColumnName);
                    continue;
                }

                // Length validation (only for string-like columns)
                if (kv.Value != null &&
                    columnMeta.MaxLength.HasValue &&
                    columnMeta.MaxLength > 0 &&
                    kv.Value is string strValue &&
                    strValue.Length > columnMeta.MaxLength)
                {
                    result.LengthViolations.Add(new LengthViolation
                    {
                        ColumnName = dbColumnName,
                        MaxLength = columnMeta.MaxLength.Value,
                        ActualLength = strValue.Length
                    });
                }
            }

            return result;
        }
        private static string ToDbColumnName(string pascalName)
        {
          

            var sb = new StringBuilder();
            for (int i = 0; i < pascalName.Length; i++)
            {
                if (char.IsUpper(pascalName[i]) && i > 0)
                    sb.Append('_');

                sb.Append(char.ToUpper(pascalName[i]));
            }
            return sb.ToString();
        }

        private static object? ConvertJsonElement(object value)
        {
            if (value is not JsonElement json)
                return value;

            return json.ValueKind switch
            {
                JsonValueKind.String =>
                    json.TryGetDateTime(out var dt) ? dt : json.GetString(),

                JsonValueKind.Number =>
                    json.TryGetInt64(out var l) ? l :
                    json.TryGetDecimal(out var d) ? d :
                    json.GetDouble(),

                JsonValueKind.True => true,
                JsonValueKind.False => false,

                JsonValueKind.Null => DBNull.Value,

                _ => json.GetRawText() // fallback (JSON column / NVARCHAR)
            };
        }




    }
}