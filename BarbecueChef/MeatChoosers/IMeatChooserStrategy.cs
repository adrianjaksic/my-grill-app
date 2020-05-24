using BarbecueChef.Models;
using System.Collections.Generic;

namespace BarbecueChef.MeatChoosers
{
    public interface IMeatChooserStrategy
    {
        Meat GetNextMeat(ICollection<Meat> meats, int maxLength, int maxWidth, out bool rotate);
    }
}
