using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;

namespace TrackWare.Application.Interfaces
{
    public interface IListOptionsRepository
    {
       Task< ListOptionsDTO >GetSettings(string typeCode,string loginID);

        Task<List<ListUspDTO>> GetLists(string typeCode, string loginID);
    }
}
