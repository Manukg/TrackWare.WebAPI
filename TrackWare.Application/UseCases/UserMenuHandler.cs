using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWare.Application.DTOs;
using TrackWare.Application.Interfaces;
using TrackWare.Domain.Entities;

namespace TrackWare.Application.UseCases
{
    public class UserMenuHandler: IUserMenuHandler
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IConfiguration _config;

        public UserMenuHandler(IMenuRepository menuRepository, IConfiguration config)
        {
            _menuRepository = menuRepository;
            _config = config;

        }

        public async Task<List<MenuItemDTO>> Handle(LoginRequestDto request)
        {
            var menuItemList = await _menuRepository.GetMenu(request.TypeCode, request.UserName);

            var dtoList = BuildHierarchy(menuItemList);

            return dtoList;

        }
        private MenuItemDTO MapToDto(MenuItem item)
        {
            return new MenuItemDTO
            {
                Label = item.MenuCaption,
                Icon = item.IconComponentName,
                Url = item.Url,
                IsCollapsed = item.IsTitle == 1,
                IsTitle = item.IsTitle == 1,
                QueryString=item.Arg,
                Children = new List<MenuItemDTO>()
            };
        }
        private List<MenuItemDTO> BuildHierarchy(List<MenuItem> items)
        {
            var lookup = items.ToDictionary(x => x.Order);
            var dtoLookup = items.ToDictionary(
                x => x.Order,
                x => MapToDto(x)
            );

            List<MenuItemDTO> roots = new();

            foreach (var item in items)
            {
                if (item.ParentID == "0")   // root item
                {
                    roots.Add(dtoLookup[item.Order]);
                }
                else
                {
                    // Attach item to its parent
                    if (dtoLookup.ContainsKey(item.ParentID))
                    {
                        dtoLookup[item.ParentID].Children.Add(dtoLookup[item.Order]);
                    }
                }
            }

            // 👉 After building the tree, convert empty lists to null
            foreach (var dto in dtoLookup.Values)
            {
                if (dto.Children != null && dto.Children.Count == 0)
                {
                    dto.Children = null;
                }
            }

            return roots;
        }

    }
}
