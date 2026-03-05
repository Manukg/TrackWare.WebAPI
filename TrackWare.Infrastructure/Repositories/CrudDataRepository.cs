using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using TrackWare.Domain.Entities;

namespace TrackWare.Infrastructure.Repositories
{
    public class CrudDataRepository : ICrudDataRepository
    {
        private IDbConnection _db;
        public CrudDataRepository(IDbConnection db)
        {
            this._db = db;
        }
        public async Task<object?> GetByIdAsync(
         CrudSettings tc,
         CrudDataRequestDto req)
        {
            var selectClause = await BuildSelectClauseAsync(tc);
            var whereClause = BuildWhereClause(tc);

            var sql = $"""
            SELECT {selectClause}
            FROM {tc.TableName}
            {whereClause}
        """;

            var result = await _db.QueryFirstOrDefaultAsync(
                sql,
                new
                {
                    Id = req.Id,
                    CmpCode = req.CMPCode,
                    YearCode = req.YearCode,
                    TypeCode = tc.TypeCode
                });

            return result;
        }
        public async Task<object?> GetEmptyAsync(
     CrudSettings tc
    )
        {
            var columns = await GetTableColumnsAsync(tc.TableName);

            IDictionary<string, object?> result = new ExpandoObject();

            foreach (var col in columns)
            {
                var propertyName = ToPascalAlias(col, tc.Prefix);
                result[propertyName] = null;
            }

            // 🔹 Hardcoded defaults for NEW mode
            result["TypeCode"] = tc.TypeCode;

            if (tc.IsCompanySpecific)
                result["CmpCode"] = tc.CCMPCode;

            if (tc.IsYearSpecific)
                result["YearCode"] = tc.YearCode;

            return result;


        }
        public static string ToPascalAlias(string columnName, string prefix)
        {
            // Remove prefix_
            if (!string.IsNullOrEmpty(prefix))
            {
                var pref = prefix + "_";
                if (columnName.StartsWith(pref, StringComparison.OrdinalIgnoreCase))
                    columnName = columnName.Substring(pref.Length);
            }

            var parts = columnName
                .Split('_', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => char.ToUpper(p[0]) + p.Substring(1).ToLower());

            return string.Concat(parts);
        }


        private async Task<List<string>> GetTableColumnsAsync(string tableName)
        {
            const string sql = """
        SELECT COLUMN_NAME
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME = @TableName
        ORDER BY ORDINAL_POSITION
    """;

            var columns = await _db.QueryAsync<string>(sql, new { TableName = tableName });
            return columns.ToList();
        }


        private async Task<string> BuildSelectClauseAsync(CrudSettings tc )
        {
            var columns = await GetTableColumnsAsync(tc.TableName);

            var selectColumns = new List<string>();

            foreach (var col in columns)
            {
                

                var alias = ToPascalAlias(col, tc.Prefix);
                selectColumns.Add($"[{col}] AS [{alias}]");
            }

            // ✅ ALWAYS hardcoded in NEW mode
          

            return string.Join(", ", selectColumns);
        }



        private string BuildWhereClause(CrudSettings tc)
        {
            var conditions = new List<string>
    {
        $"{tc.Prefix}_id = @Id"
    };

            conditions.Add($"{tc.Prefix}_type_code = @TypeCode");
            if (tc.IsCompanySpecific)
                conditions.Add($"{tc.Prefix}_cmpcode = @CmpCode");

            if (tc.IsYearSpecific)
                conditions.Add($"{tc.Prefix}_yearcode = @YearCode");

            return "WHERE " + string.Join(" AND ", conditions);
        }

    }
}