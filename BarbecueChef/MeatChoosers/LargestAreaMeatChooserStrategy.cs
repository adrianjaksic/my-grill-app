using BarbecueChef.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarbecueChef.MeatChoosers
{
    public class LargestAreaMeatChooserStrategy : IMeatChooserStrategy
    {
        public Meat GetNextMeat(ICollection<Meat> meats, int maxLength, int maxWidth, out bool rotate)
        {
            var meat1 = meats.Where(m => m.NotFinished && m.Length <= maxLength && m.Width <= maxWidth).OrderByDescending(m => m.Area).FirstOrDefault();
            var meat2 = meats.Where(m => m.NotFinished && m.Width <= maxLength && m.Length <= maxWidth).OrderByDescending(m => m.Area).FirstOrDefault();

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

            if (meat2.Area > meat1.Area)
            {
                rotate = true;
                return meat2;
            }
            rotate = false;
            return meat1;
        }
    }
}
