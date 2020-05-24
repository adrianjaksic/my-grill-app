namespace BarbecueChef.Models
{
    public class Meat
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Time { get; set; }
        public int Quantity { get; set; }
        public int PrepearedQuantity { get; set; }

        public Menu Menu { get; set; }

        public bool NotFinished => Quantity > PrepearedQuantity;
        public int Area => Length * Width;
    }
}
