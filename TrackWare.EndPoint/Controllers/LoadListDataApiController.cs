using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;

namespace TrackWare.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadListDataApiController : ControllerBase
    {

        private readonly IListDataHandler _listDataHandler;
        //    private readonly IDbConnection _dbConnection;
        public LoadListDataApiController(IListDataHandler listDataHandler)
        {
            this._listDataHandler = listDataHandler;
            //  _dbConnection = dbConnection;
        }

        [HttpPost("ShowData")]
        [Authorize]
        public async Task<IActionResult> ShowData(ListParamDTO arg)
        {
            var loginID = User.FindFirst("loginID")?.Value;
            var cmpCode = User.FindFirst("companyID")?.Value;
            arg.loginID = loginID;
            arg.CMPCode = cmpCode;
            var res = await _listDataHandler.GetData(arg);



            return Ok(res);
        }
        [HttpPost("GetAllGridColumns")]
        [Authorize]
        public async Task<IActionResult> GetAllGridColumns([FromBody] GridLayoutArgDTO arg)
        {
            try
            {
                var loginID = User.FindFirst("loginID")?.Value;
                var cmpCode = User.FindFirst("companyID")?.Value;

                arg.loginID = loginID;
                arg.CMPCode = cmpCode;

                var res = await _listDataHandler.GetColumnNames(arg);

                return Ok(res);
            }
            catch (Exception ex)
            {
                // log the exception (optional, but recommended)
               // _logger.LogError(ex, "Error occurred in GetAllGridColumns");

                // return safe error message to client
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred while retrieving grid columns." });
            }
        }


        [HttpPost("GetColumnSettings")]
        [Authorize]
        public async Task<IActionResult> GetColumnSettings(GridLayoutArgDTO arg)
        {
            var loginID = User.FindFirst("loginID")?.Value;
            var cmpCode = User.FindFirst("companyID")?.Value;
            arg.loginID = loginID;
            arg.CMPCode = cmpCode;
            var res = await _listDataHandler.GetColumnSettings(arg);



            return Ok(res);
        }


        [HttpPost("ApplyColumnSettings")]
        [Authorize]
        public async Task<IActionResult> ApplyColumnSettings(GridLayoutSettingsDTO arg)
        {
            var loginID = User.FindFirst("loginID")?.Value;
            var cmpCode = User.FindFirst("companyID")?.Value;
            arg.LoginID = loginID;
            var res = await _listDataHandler.ApplyColumnSettings(arg);



            return Ok(res);
        }

    }
}
