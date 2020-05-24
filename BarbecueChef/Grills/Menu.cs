using System.Collections.Generic;

namespace BarbecueChef.Grills
{
    public class Menu
    {
        public int SortOrder { get; set; }
        public string MenuId { get; set; }
        public string Name { get; set; }
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
