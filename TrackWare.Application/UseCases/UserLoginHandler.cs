using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using TrackWare.Domain.Entities;
 
using System.Security.Claims;
using System.Text; 

using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace TrackWare.Application.UseCases
{
    public class UserLoginHandler: IUserLoginHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public UserLoginHandler(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;

        }

        public virtual async Task<LoginResponseDto> Handle(LoginRequestDto request)
        {
            var user = await _userRepository.GetByUserNameAsync(request.TypeCode, request.UserName);

            if (user == null || !VerifyPassword(request.Password, user.LoginPassword))
            {
                return new LoginResponseDto
                {
                    IsAuthenticated = false
                };
            }

            return new LoginResponseDto
            {
                UserId = user.Id,
                UserName = user.LoginId,
                IsAuthenticated = true,
                Token = GenerateToken(user) // Optional JWT token
            };
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using var sha256 = SHA256.Create();
            var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return hash == storedHash;
        }
        private string GenerateToken(User user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user), "User object cannot be null.");

                // Read configuration values
                var key = _config["Jwt:Key"];
                var issuer = _config["Jwt:Issuer"];

                if (string.IsNullOrWhiteSpace(key))
                    throw new InvalidOperationException("JWT Key is missing in configuration.");
                if (string.IsNullOrWhiteSpace(issuer))
                    throw new InvalidOperationException("JWT Issuer is missing in configuration.");

                // Create signing key & credentials
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // Define claims
                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, user.LoginId ?? string.Empty),
            new Claim("userId", user.LoginId ?? string.Empty),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                // Create token
                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: null,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: credentials);

                // Return serialized token
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log the error (replace with ILogger if available)
                Console.WriteLine($"Error generating token: {ex}");

                // Rethrow to propagate error
                throw;
            }
        }

    }
}
