using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;

namespace TrackWare.Application.UseCases
{
    public class ListHandler:IListHandler
    {
    private readonly    IListOptionsRepository _listOptionsRepository;
        public ListHandler(IListOptionsRepository listOptionsRepository)
        {
            _listOptionsRepository = listOptionsRepository;
        }
        public async Task<ListOptionsDTO> GetListSettings(string typeCode, string loginID)
        {
            // await Task.Delay(1000);
            var settings = await _listOptionsRepository.GetSettings(typeCode, loginID);
            var lists = await _listOptionsRepository.GetLists(typeCode, loginID);
            settings.ListProc = lists;
            return settings;

        }
    }
}
