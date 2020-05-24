using BarbecueChef.Grills;
using ConsoleApp.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace ConsoleApp.Services
{
    public class MenuRequestService : IMenuService
    {
        private const string MenuEndpoint = "http://isol-grillassessment.azurewebsites.net/api/GrillMenu";

        public List<Menu> GetMenus()
        {
            using (var client = new HttpClient())
            {
                var result = client.GetAsync(MenuEndpoint).Result;
                using (HttpContent content = result.Content)
                {
                    string data = content.ReadAsStringAsync().Result;
                    if (data != null)
                    {
                        using (var jsonDoc = JsonDocument.Parse(data))
                        {
                            var root = jsonDoc.RootElement;
                            return DeserializeMenu(root);
                        }
                    }
                }
            }
            return new List<Menu>();
        }

        private List<Menu> DeserializeMenu(JsonElement root)
        {
            var menus = new List<Menu>();
            foreach (var menuItem in root.EnumerateArray())
            {
                var newMenu = new Menu();
                newMenu.MenuId = menuItem.GetProperty("Id").GetString();
                newMenu.Name = menuItem.GetProperty("menu").GetString();
                var items = menuItem.GetProperty("items");
                for (var i = 0; i < items.GetArrayLength(); i++)
                {
                    Meat meat = DeserializeMeat(items[i], i);
                    meat.Menu = newMenu;
                    newMenu.Items.Add(meat);
                }
                menus.Add(newMenu);
            }
            return menus;
        }

        private Meat DeserializeMeat(JsonElement item, int i)
        {
            return new Meat()
            {
                Id = item.GetProperty("Id").GetString(),
                Name = item.GetProperty("Name").GetString(),
                Length = item.GetProperty("Length").GetInt32(),
                Width = item.GetProperty("Width").GetInt32(),
                Time = DeserializeTime(item.GetProperty("Duration").GetString()),
                Quantity = item.GetProperty("Quantity").GetInt32(),
                SortedQuantity = 0,
                PrepearedQuantity = 0,
            };
        }

        private int DeserializeTime(string stringTime)
        {
            var split = stringTime.Split(':');
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            if (split.Length == 3)
            {
                int.TryParse(split[0], out hours);
            }
            if (split.Length >= 2)
            {
                int.TryParse(split[1], out minutes);
            }
            if (split.Length >= 1)
            {
                int.TryParse(split[2], out seconds);
            }
            return hours * 60 * 60 + minutes * 60 + seconds;                
        }
    }
}
