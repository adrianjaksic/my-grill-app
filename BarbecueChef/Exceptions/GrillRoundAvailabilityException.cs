using System;

namespace BarbecueChef.Exceptions
{
    public class GrillRoundAvailabilityException : ApplicationException
    {
        public GrillRoundAvailabilityException() : base("No available space for this meat.")
        {

        }
    }
}
