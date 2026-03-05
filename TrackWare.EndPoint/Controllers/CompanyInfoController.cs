using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;

namespace TrackWare.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyInfoController : ControllerBase
    {
        ICompanyInfoHandle _companyInfoHandle;
        public CompanyInfoController(ICompanyInfoHandle companyInfoHandle)
        {
            this._companyInfoHandle = companyInfoHandle;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            
             var res =  await _companyInfoHandle.LoadCompanyList();

            var dynamicArray = res.Select(kvp =>
            {
                dynamic obj = new ExpandoObject();
                obj.code = kvp.Key;
                obj.companyName = kvp.Value;
                return obj;
            }).ToArray();
            var retObj = new
            {
                companyList = dynamicArray,
                ProductName = "Trackware WMS",
                LicCompany = "XYZ Enterprises LLC",
                ExpiryDate = "01-Jan-2028",
                ShowCompany = true,
                DefaultCompany=100
            };

            return Ok(retObj);
        }
    }
}
