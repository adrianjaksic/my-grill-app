using BarbecueChef.Grills;
using System.Collections.Generic;

namespace ConsoleApp.Interfaces
{
    public interface IMenuService
    {
        List<Menu> GetMenus();
    }
}
