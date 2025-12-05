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
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            Console.WriteLine(dbConnection.ConnectionString);
            _dbConnection = dbConnection;
        }

        public async Task<User?> GetByUserNameAsync(string typeCode, string userName)
        {
            const string sql = @"SELECT USR_ID as ID,USR_USERFULLNAME as FullName,USR_EMAILADDRESS as EmailAddress,USR_MOBILENUMBER as MobileNumber,USR_ROLE_ID AS RoleID ,USR_LOGINID LoginId,USR_LOGINPASSWORD LoginPassword,USR_CREATEBY CreateBy,USR_CREATETS CreateTs 
                             FROM USER_PROFILE 
                             WHERE USR_TYPECODE =@TypeCode AND USR_LOGINID = @UserName";

            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { TypeCode = typeCode, UserName = userName });
        }
    }
}
