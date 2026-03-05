using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.Interfaces;
using TrackWare.Domain.Entities;

namespace TrackWare.Infrastructure.Repositories
{
    public class RoleRepository: IRoleRepository
    {
         private readonly IDbConnection _dbConnection;

        public RoleRepository(IDbConnection dbConnection)
        {
            Console.WriteLine(dbConnection.ConnectionString);
            _dbConnection = dbConnection;
        }

        public async Task<RoleSettings?> GetRoleSettings(string typeCode, string roleId)
        {
            const string sql = @"
        SELECT 
            ROLE_TYPE_CODE       AS TypeCode,
            ROLE_ID              AS Id,
            ROLE_DESCRIPTION     AS Description,
            ROLE_CREATE_TS       AS CreateTs,
            ROLE_CREATE_BY       AS CreateBy,
            ROLE_APPROVAL_STATUS AS ApprovalStatus,
            ROLE_ADD_STATUS      AS AddStatus,
            ROLE_EDIT_STATUS     AS EditStatus,
            ROLE_DELETE_STATUS   AS DeleteStatus,
            ROLE_PRINT_STATUS    AS PrintStatus,
            ROLE_APPROVE_STATUS  AS ApproveStatus,
            ROLE_ALLOW_LAYOUT    AS AllowLayout,
            ROLE_ALLOW_CANCEL_POSTING AS AllowCancelPosting,
            ROLE_VIEW_SELF_CREATED_ONLY AS ViewSelfCreatedOnly
        FROM ROLE_SETTINGS
        WHERE ROLE_TYPE_CODE = @TypeCode
          AND ROLE_ID = @RoleId";

            var role = await _dbConnection.QueryFirstOrDefaultAsync<RoleSettings>(
                sql,
                new { TypeCode = typeCode, RoleId = roleId }
            );

            return role;
        }

        public async Task<List<RoleSettings>> GetAllRoleSettings(string typeCode)
        {
            const string sql = @"
        SELECT 
            ROLE_TYPE_CODE       AS TypeCode,
            ROLE_ID              AS Id,
            ROLE_DESCRIPTION     AS Description,
            ROLE_CREATE_TS       AS CreateTs,
            ROLE_CREATE_BY       AS CreateBy,
            ROLE_APPROVAL_STATUS AS ApprovalStatus,
            ROLE_ADD_STATUS      AS AddStatus,
            ROLE_EDIT_STATUS     AS EditStatus,
            ROLE_DELETE_STATUS   AS DeleteStatus,
            ROLE_PRINT_STATUS    AS PrintStatus,
            ROLE_APPROVE_STATUS  AS ApproveStatus,
            ROLE_ALLOW_LAYOUT    AS AllowLayout,
            ROLE_ALLOW_CANCEL_POSTING AS AllowCancelPosting,
            ROLE_VIEW_SELF_CREATED_ONLY AS ViewSelfCreatedOnly
        FROM ROLE_SETTINGS
        WHERE ROLE_TYPE_CODE = @TypeCode
        ORDER BY ROLE_ID";

            var list = await _dbConnection.QueryAsync<RoleSettings>(
                sql,
                new { TypeCode = typeCode }
            );

            return list.ToList();
        }

    }
}
