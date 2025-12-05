using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;

namespace TrackWare.Application.Interfaces
{
    public interface IUserLoginHandler
    {
        Task<LoginResponseDto> Handle(LoginRequestDto request);
    }
}
