using Newtonsoft.Json;
using System.Collections.Generic;

namespace BarbecueChef.Grills
{
    public class Menu
    {
        public int SortOrder { get; set; }
        
        [JsonProperty("Id")]
        public string Id { get; set; }
        
        [JsonProperty("menu")]
        public string Name { get; set; }

        [JsonProperty("items")]
        public List<Meat> Items { get; private set; } = new List<Meat>();
        public bool Finished => GetFinished();

        private bool GetFinished()
        {
            foreach (var item in Items)
            {
                if (!item.Finished)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
