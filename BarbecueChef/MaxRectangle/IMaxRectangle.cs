namespace BarbecueChef.MaxRectangle
{
    public interface IMaxRectangle
    {
        /// <summary>
        /// Gets the max free rectangle for the 2D surface. False is free.
        /// </summary>
        /// <param name="surface">2D surface</param>
        /// <returns>Max free rectangle</returns>
        Rectangle GetLargestRectangle(bool[,] surface);
    }
}
