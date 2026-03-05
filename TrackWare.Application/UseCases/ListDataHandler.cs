using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;

namespace TrackWare.Application.UseCases
{
    public class ListDataHandler:IListDataHandler
    {
        IGridDataProvider _gridDataProvider;
        IColumnSchemaProvider _columnSchemaProvider;
        public ListDataHandler(IGridDataProvider gridDataProvider, IColumnSchemaProvider columnSchemaProvider)
        {

            this._gridDataProvider = gridDataProvider;
            this._columnSchemaProvider = columnSchemaProvider;
        }

        public async Task<bool> ApplyColumnSettings(GridLayoutSettingsDTO arg)
        {
            
            await this._columnSchemaProvider.SaveLayout(arg);
            return true;
        }

        public async Task<GridLayoutSettingsDTO> GetColumnNames(GridLayoutArgDTO arg)
        {
            return await this._columnSchemaProvider.GetColumnAsync(arg);
        }

        public async Task<GridLayoutSettingsDTO> GetColumnSettings(GridLayoutArgDTO arg)
        {
          return  await this._columnSchemaProvider.ShowAvailableColumns(arg);
             
        }

        public async Task<object> GetData(ListParamDTO arg) {
            return await this._gridDataProvider.GetDataAsync(arg);
        }

      
    }
}
