using BarbecueChef.Grills;
using System.Collections.Generic;
using System.Linq;

namespace BarbecueChef.MeatChoosers
{
    public class LargestWidthMeatChooserStrategy : IMeatChooserStrategy
    {
        public Meat GetNextMeat(ICollection<Meat> meats, int maxLength, int maxWidth, out bool rotate)
        {
            var meat1 = meats.Where(m => !m.Sorted && m.Length <= maxLength && m.Width <= maxWidth).OrderByDescending(m => m.Width).ThenByDescending(m => m.Length).FirstOrDefault();
            var meat2 = meats.Where(m => !m.Sorted && m.Width <= maxLength && m.Length <= maxWidth).OrderByDescending(m => m.Length).ThenByDescending(m => m.Width).FirstOrDefault();

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
