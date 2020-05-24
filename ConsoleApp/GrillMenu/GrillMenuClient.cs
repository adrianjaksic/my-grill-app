using BarbecueChef.Grills;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace ConsoleApp.GrillMenu
{
    public class GrillMenuClient : ServiceClient<GrillMenuClient>
    {
        public ServiceClientCredentials Credentials { get; }
        public Uri BaseUri
        {
            get { return HttpClient.BaseAddress; }
        }

        private const string DefaultBaseUri = "http://isol-grillassessment.azurewebsites.net";
        private const double HttpClientTimeoutInSeconds = 30;

        public GrillMenuClient(ServiceClientCredentials credentials, params DelegatingHandler[] handlers) : base(handlers)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException("credentials");
            }

            Credentials = credentials;
            Credentials.InitializeServiceClient(this);            
            HttpClient.Timeout = TimeSpan.FromSeconds(HttpClientTimeoutInSeconds);
            HttpClient.BaseAddress = new Uri(DefaultBaseUri);
        }

        public List<Menu> GetAll()
        {
            const string url = "/api/GrillMenu";
            var result = HttpClient.GetAsync(url).Result;
            using (HttpContent content = result.Content)
            {
                string data = content.ReadAsStringAsync().Result;
                if (data != null)
                {
                    using (var jsonDoc = JsonDocument.Parse(data))
                    {
                        var root = jsonDoc.RootElement;
                        return DeserializeMenu(root).OrderBy(m => m.Name).ToList();
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
