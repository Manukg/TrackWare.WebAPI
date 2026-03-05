using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Domain.Entities;

namespace TrackWare.Application.Interfaces
{
    public interface ICompanyProfileRepositor
    {


         Task<List<CompanyProfile>> GetCompanyProfileList();
        
    }
}
