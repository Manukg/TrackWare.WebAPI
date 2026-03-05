using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.Interfaces;

namespace TrackWare.Application.UseCases
{
    public class CompanyInfoHandle : ICompanyInfoHandle
    {

        private readonly IConfiguration _config;
        private readonly ICompanyProfileRepositor _companyRepository;

        public CompanyInfoHandle( IConfiguration config, ICompanyProfileRepositor companyRepository)
        {
           
            this._config = config;
            this._companyRepository = companyRepository;
        }

        public async   Task <Dictionary<string, string>> LoadCompanyList()
        {
            var list =  await this._companyRepository.GetCompanyProfileList();

            var res = list.ToDictionary(c => c.Id.ToString(), c => c.Name);

            return res;
 
        }
 
    }
}
