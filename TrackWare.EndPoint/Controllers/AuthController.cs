using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using TrackWare.Application.UseCases;

namespace TrackWare.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserLoginHandler _userLoginHandler;
    //    private readonly IDbConnection _dbConnection;
        public AuthController(IUserLoginHandler userLoginHandler)
        {
            _userLoginHandler = userLoginHandler;
          //  _dbConnection = dbConnection;
        }

        /// <summary>
        /// User Login -> Returns JWT Token
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Call your handler to process the login
            var result = await _userLoginHandler.Handle(request);


            if (!result.IsAuthenticated)
                return Unauthorized(new { message = "Invalid User Name or Password" });

            return Ok(new
            {
                token = result.Token,
                expiresIn = 3200,
                user = result.UserName
            });
        }

    }
}
