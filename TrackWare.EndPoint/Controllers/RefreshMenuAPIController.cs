using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using TrackWare.Application.Interfaces;
using TrackWare.Application.UseCases;

namespace TrackWare.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshMenuAPIController : ControllerBase
    {

        private readonly IUserMenuHandler _usermenuHandler;
        //    private readonly IDbConnection _dbConnection;
        public RefreshMenuAPIController(IUserMenuHandler usermenuHandler)
        {
           this. _usermenuHandler = usermenuHandler;
            //  _dbConnection = dbConnection;
        }

        [HttpGet("GetMenu")]
        [Authorize]
        public async Task<IActionResult> GetMenu()
        {
            var loginID = User.FindFirst("loginID")?.Value;
            var res = await _usermenuHandler.Handle(new Application.DTOs.LoginRequestDto {TypeCode="USR",UserName= loginID });

         

            return Ok(res);
        }

    }
}
