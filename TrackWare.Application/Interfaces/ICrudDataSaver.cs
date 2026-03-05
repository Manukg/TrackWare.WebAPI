using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;

namespace TrackWare.Application.Interfaces
{
    public interface ICrudDataSaver
    {
        Task<SaveResultDTO> SaveDataAsync(bool isNew, string jsonData,string logiID);
    }
}
