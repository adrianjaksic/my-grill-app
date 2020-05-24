using BarbecueChef.Exceptions;
using BarbecueChef.Models;
using System.Collections.Generic;

namespace BarbecueChef.GrillRounds
{
    public class GrillRound
    {
        public int RoundNumber { get; private set; }
        public List<MeatRound> Items { get; private set; } = new List<MeatRound>();
        public bool AllFinished => GetAllFinished();
        public int GrillSurfaceArea => Surface.GetLength(0) * Surface.GetLength(1);
        public int GrillUsedArea => GetGrillUsedArea();

        public bool[,] Surface { get; set; }

        public GrillRound(int roundNumber, int length, int width)
        {
            RoundNumber = roundNumber;
            Surface = new bool[length, width];
        }

        /// <summary>
        /// Adding meat to the grill round. Two meals can't overlap.
        /// </summary>
        /// <param name="meat">Meat</param>
        /// <param name="rotate">If the meat should be rotated</param>
        /// <param name="lengthPosition">Length position of meat on the surface</param>
        /// <param name="widthPosition">Width position of meat on the surface</param>
        /// <exception cref="GrillRoundAvailabilityException">Thrown if no space</exception>
        public void AddMeat(Meat meat, bool rotate, int lengthPosition, int widthPosition)
        {
            var meatLength = rotate ? meat.Width : meat.Length;
            var meatWidth = rotate ? meat.Length : meat.Width;
            if (!CheckAvailability(lengthPosition, widthPosition, meatLength, meatWidth))
            {
                throw new GrillRoundAvailabilityException();
            }
            AddToSurface(lengthPosition, widthPosition, meatLength, meatWidth);
            Items.Add(new MeatRound() 
            {
                Meat = meat,
                TimeUntilFinished = meat.Time,
                LengthPosition = lengthPosition,
                WidthPosition = widthPosition,
                Length = meatLength,
                Width = meatWidth,
            });
        }

        /// <summary>
        /// Use for "cooking" the meat. After each time interval check if meats are finished.
        /// </summary>
        /// <param name="time">Time interval that passed from last call.</param>
        public void TimeElapsed(int time)
        {
            foreach (var item in Items)
            {
                if (item.TimeUntilFinished > 0)
                {
                    item.TimeUntilFinished -= time;
                    if (item.TimeUntilFinished <= 0)
                    {
                        item.Meat.PrepearedQuantity += 1;
                        RemoveFromSurface(item.LengthPosition, item.WidthPosition, item.Length, item.Width);
                    }
                }
            }
        }

        private bool CheckAvailability(int lengthPosition, int widthPosition, int length, int width)
        {
            if (lengthPosition < 0 || length < 0 || lengthPosition + length > Surface.GetLength(0))
            {
                return false;
            }
            if (widthPosition < 0 || width < 0 || widthPosition + width > Surface.GetLength(1))
            {
                return false;
            }
            for (int i = lengthPosition; i < lengthPosition + length; i++)
            {
                for (int j = widthPosition; j < widthPosition + width; j++)
                {
                    if (Surface[i,j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void AddToSurface(int lengthPosition, int widthPosition, int length, int width)
        {
            for (int i = lengthPosition; i < lengthPosition + length; i++)
            {
                for (int j = widthPosition; j < widthPosition + width; j++)
                {
                    Surface[i, j] = true;
                }
            }
        }

        private void RemoveFromSurface(int lengthPosition, int widthPosition, int length, int width)
        {
            for (int i = lengthPosition; i < lengthPosition + length; i++)
            {
                for (int j = widthPosition; j < widthPosition + width; j++)
                {
                    Surface[i, j] = false;
                }
            }
        }

        private bool GetAllFinished()
        {
            var allFinished = true;
            foreach (var item in Items)
            {
                if (item.TimeUntilFinished > 0)
                {
                    allFinished = false;
                }
            }
            return allFinished;
        }

        private int GetGrillUsedArea()
        {
            int area = 0;
            for (int i = 0; i < Surface.GetLength(0); i++)
            {
                for (int j = 0; j < Surface.GetLength(1); j++)
                {
                    if (Surface[i, j])
                    {
                        area++;
                    }
                }
            }
            return area;
        }
    }
}
