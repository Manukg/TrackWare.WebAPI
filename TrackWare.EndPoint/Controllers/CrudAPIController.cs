using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using TrackWare.Application.UseCases;
using TrackWare.Domain.Entities;

namespace TrackWare.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CrudAPIController : ControllerBase
    {
        private readonly ICRUDHelper _crudHelper;
        public CrudAPIController(ICRUDHelper crudHelper)
        {
            this._crudHelper = crudHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CrudDataRequestDto request)
        {
            var loginID = User.FindFirst("loginID")?.Value;
            var companyID = User.FindFirst("companyID")?.Value;
            var yearCode = User.FindFirst("yearCode")?.Value;

            request.LoginID = loginID;
            request.CMPCode = companyID;
            request.YearCode = yearCode;

            
                var response = await _crudHelper.ResolveAsync(request);
            
               
            // var response = await _crudHelper.ResolveAsync<object, object>(request);
            return Ok(response);
        }

     
    }
}
