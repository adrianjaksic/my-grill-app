using System.Collections.Generic;

namespace BarbecueChef.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public List<Meat> Items { get; private set; } = new List<Meat>();

        public bool IsFinished()
        {
            foreach (var item in Items)
            {
                if (item.Quantity > item.PrepearedQuantity)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
