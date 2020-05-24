using BarbecueChef.Grills;
using System.Collections.Generic;
using System.Linq;

namespace BarbecueChef.MeatChoosers
{
    public class LargestWidthMenuPriorityMeatChooserStrategy : IMeatChooserStrategy
    {
        public Meat GetNextMeat(ICollection<Meat> meats, int maxLength, int maxWidth, out bool rotate)
        {
            if (meats.Count == 0)
            {
                rotate = false;
                return null;
            }

            var noRotateMeats = meats.Where(m => !m.Sorted && m.Length <= maxLength && m.Width <= maxWidth);
            var minMenuSortOrder = noRotateMeats.Any() ? noRotateMeats.Min(m => m.Menu.SortOrder) : 0;
            var meat1 = noRotateMeats.OrderByDescending(m => m.Menu.SortOrder == minMenuSortOrder).ThenByDescending(m => m.Width).ThenByDescending(m => m.Length).FirstOrDefault();
            
            var rotateMeats = meats.Where(m => !m.Sorted && m.Width <= maxLength && m.Length <= maxWidth);
            var minMenuSortOrderWithRotate = rotateMeats.Any() ? rotateMeats.Min(m => m.Menu.SortOrder) : 0;
            var meat2 = rotateMeats.OrderByDescending(m => m.Menu.SortOrder == minMenuSortOrderWithRotate).ThenByDescending(m => m.Length).ThenByDescending(m => m.Width).FirstOrDefault();

            if (meat1 == null)
            {
                rotate = true;
                return meat2;
            }
            if (meat2 == null)
            {
                rotate = false;
                return meat1;
            }

            if (meat2.Length > meat1.Width)
            {
                rotate = true;
                return meat2;
            }
            if (meat2.Length == meat1.Width)
            {
                if (meat2.Width == meat1.Length)
                {
                    rotate = true;
                    return meat2;
                }
            }
            rotate = false;
            return meat1;
        }
    }
}
