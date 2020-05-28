using BarbecueChef.Grills;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Web.Script.Serialization;

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
                    var rez = JsonConvert.DeserializeObject<List<Menu>>(data).OrderBy(m => m.Name).ToList();
                    rez.ForEach(m => m.Items.ForEach(i => UpdateTimeAndMenu(i, m)));
                    return rez;
                }
            }
            return new List<Menu>();
        }

        private void UpdateTimeAndMenu(Meat meat, Menu menu)
        {
            meat.Menu = menu;
            var split = meat.Duration.Split(':');
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
            meat.Time = hours * 60 * 60 + minutes * 60 + seconds;
        }
    }
}
