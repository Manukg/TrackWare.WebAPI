using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TrackWare.Application.DTOs;

namespace TrackWare.Application.Interfaces
{
    public interface IListDataHandler
    {
        Task<object> GetData(ListParamDTO arg);

        Task<GridLayoutSettingsDTO> GetColumnNames(GridLayoutArgDTO arg);

       Task<bool> ApplyColumnSettings(GridLayoutSettingsDTO arg);

        Task<GridLayoutSettingsDTO> GetColumnSettings(GridLayoutArgDTO arg);


    }
}
