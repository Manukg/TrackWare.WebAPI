using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;

namespace TrackWare.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CrudDataSaveAPIController : ControllerBase
    {
        private readonly ICRUDHelper _crudHelper;
        public CrudDataSaveAPIController(ICRUDHelper crudHelper)
        {
            this._crudHelper = crudHelper;
        }

        // Added explicit route to avoid ambiguous POST routes.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SaveEntryRequestDto request)
        {
            var loginID = User.FindFirst("loginID")?.Value;
            var companyID = User.FindFirst("companyID")?.Value;
            var yearCode = User.FindFirst("yearCode")?.Value;

            // Convert the incoming request object to a JSON string.
            // Use System.Text.Json for serialization with camel-case property names.
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            string json = JsonSerializer.Serialize(request.Data, jsonOptions);

            // You can pass the JSON string to your helper or keep passing the original object.
            // Here we pass the JSON string as the request payload.
            var response = await _crudHelper.SaveDataAsync(request.IsNew, json, loginID);
            // var response = await _crudHelper.ResolveAsync<object, object>(request);
            return Ok(response);
        }
    }
}
