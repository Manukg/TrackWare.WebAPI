using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Domain.Entities;

namespace TrackWare.Application.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<MenuItem>?> GetMenu(string userType, string loginID);
    }
}
