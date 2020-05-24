namespace BarbecueChef.Grills
{
    public class Meat
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Time { get; set; }
        public int Quantity { get; set; }
        public int SortedQuantity { get; set; }
        public int PrepearedQuantity { get; set; }
        public Menu Menu { get; set; }

        public bool Finished => Quantity <= PrepearedQuantity;
        public bool Sorted => Quantity <= SortedQuantity;
        public int Area => Length * Width;
    }
}
