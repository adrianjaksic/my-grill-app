using BarbecueChef.Grills;

namespace BarbecueChef.GrillRounds
{
    public class MeatRound
    {
        public Meat Meat { get; set; }
        public int TimeUntilFinished { get; set; }
        public int LengthPosition { get; set; }
        public int WidthPosition { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
    }
}
