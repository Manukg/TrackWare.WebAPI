using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;

namespace TrackWare.Application.Interfaces
{
    public interface IUserMenuHandler
    {

      
        Task<List<MenuItemDTO>> Handle(LoginRequestDto request);
        
    }
}
