using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWare.Application.Interfaces
{
    public interface ICompanyInfoHandle
    {
        Task<Dictionary<string, string>> LoadCompanyList();
    }
}
