using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using TrackWare.Application.DTOs;

namespace TrackWare.Application.Interfaces
{
  public    interface IColumnSchemaProvider
    {
        Task<GridLayoutSettingsDTO> GetColumnAsync(GridLayoutArgDTO arg);

        Task SaveLayout(GridLayoutSettingsDTO arg);

        Task<GridLayoutSettingsDTO> ShowAvailableColumns(GridLayoutArgDTO arg);
    }
}
