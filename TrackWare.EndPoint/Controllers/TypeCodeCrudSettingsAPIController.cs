using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackWare.Application.Interfaces;

namespace TrackWare.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeCodeCrudSettingsAPIController : ControllerBase
    {
        
        private readonly ICRUDHelper _crudHelper;
        public TypeCodeCrudSettingsAPIController(ICRUDHelper crudHelper)
        {
            this._crudHelper = crudHelper;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(string typeCode)
        {
            var loginID = User.FindFirst("loginID")?.Value;
            var res = await this._crudHelper.ResolveTypeSettingsAysnc(typeCode, loginID);
            return Ok(res);
        }

    }
}
