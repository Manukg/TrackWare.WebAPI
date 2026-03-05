using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using TrackWare.Application.UseCases;
using TrackWare.EndPoint.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrackWare.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileDataSaveAPIController : ControllerBase
    {
        private readonly ICRUDHelper _crudHelper;
        public UserProfileDataSaveAPIController(ICRUDHelper crudHelper)
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
            var changePassword = Convert.ToBoolean( request.Data["ChangePassowrd_tmp"]?.ToString() ?? "true");
           if( changePassword || request.IsNew){
                var password = request.Data["Loginpassword"]?.ToString() ?? string.Empty;
                using var sha256 = SHA256.Create();
                var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                request.Data["Loginpassword"] = hash;
            }
           
                     
            string json = JsonSerializer.Serialize(request.Data, jsonOptions);

            // You can pass the JSON string to your helper or keep passing the original object.
            // Here we pass the JSON string as the request payload.
            var response = await _crudHelper.SaveDataAsync(request.IsNew, json, loginID);
            // var response = await _crudHelper.ResolveAsync<object, object>(request);
            return Ok(response);
        }
        [HttpPost("savefile")]
        public async Task<IActionResult> SaveFile([FromForm] UserSaveDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Form data is required.");
            }

            string fileName = string.Empty;
            string fullPath = null;
            bool fileCreated = false;

            // Local helper to attempt safe deletion of a partially written file.
            void TryDelete(string path)
            {
                try
                {
                    if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                catch
                {
                    // Swallow exceptions - best effort cleanup only.
                }
            }

            try
            {
                if (dto.UserPhoto != null && dto.UserPhoto.Length > 0)
                {
                    fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.UserPhoto.FileName)}";
                    var dir = Path.Combine("wwwroot", "userphotos");

                    // Ensure the directory exists
                    Directory.CreateDirectory(dir);

                    fullPath = Path.Combine(dir, fileName);

                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await dto.UserPhoto.CopyToAsync(stream);

                    fileCreated = true;

                    // Save fileName into USR_USER_PHOTO column (persist to DB as needed)
                }

                // Save other fields normally (caller logic)

                return Ok(fileName);
            }
            catch (UnauthorizedAccessException)
            {
                if (fileCreated && fullPath != null) TryDelete(fullPath);
                return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient permissions to save the file.");
            }
            catch (IOException)
            {
                if (fileCreated && fullPath != null) TryDelete(fullPath);
                return StatusCode(StatusCodes.Status500InternalServerError, "An I/O error occurred while saving the file.");
            }
            catch (Exception ex)
            {
                if (fileCreated && fullPath != null) TryDelete(fullPath);
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }
}
