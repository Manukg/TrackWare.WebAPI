using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackWare.Application.Interfaces;
using TrackWare.Application.UseCases;

namespace TrackWare.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListOptionsApiController : ControllerBase
    {
        private readonly IListHandler _listHandler;
        public ListOptionsApiController(IListHandler listHandler)
        {
            this._listHandler=listHandler;
        }

        [HttpGet("GetListOption")]
        [Authorize]
        public async Task<IActionResult> GetListOption(string typeCode)
        {
            var loginID = User.FindFirst("loginID")?.Value;
            var res = await this. _listHandler.GetListSettings(typeCode, loginID);
            return Ok(res);
        }
    }
}
