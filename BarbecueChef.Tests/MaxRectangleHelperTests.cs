using BarbecueChef.GrillRounds;
using BarbecueChef.MaxRectangle;
using BarbecueChef.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BarbecueChef.Tests
{
    [TestClass]
    public class MaxRectangleHelperTests
    {
        private const int GrillLength = 20;
        private const int GrillWidth = 30;

        [TestMethod]
        public void MaxSurface()
        {
            var round = GetNewGrillRound();
            var area = MaxRectangleHelper.GetLargestArea(round.Surface);
            Assert.AreEqual(GrillLength, area.Length);
            Assert.AreEqual(GrillWidth, area.Width);
            Assert.AreEqual(0, area.LengthPosition);
            Assert.AreEqual(0, area.WidthPosition);
        }

        [TestMethod]
        public void HalfSurface()
        {
            var round = GetNewGrillRound();
            round.AddMeat(GetMeat(GrillLength / 2, GrillWidth), false, 0, 0);
            var area = MaxRectangleHelper.GetLargestArea(round.Surface);
            Assert.AreEqual(GrillLength / 2, area.Length);
            Assert.AreEqual(GrillWidth, area.Width);
            Assert.AreEqual(GrillLength / 2, area.LengthPosition);
            Assert.AreEqual(0, area.WidthPosition);
        }

        [TestMethod]
        public void HalfOfHalfSurface()
        {
            var round = GetNewGrillRound();
            round.AddMeat(GetMeat(GrillLength / 2, GrillWidth), false, 0, 0);
            round.AddMeat(GetMeat(GrillLength / 2, GrillWidth / 2), false, GrillLength / 2, 0);
            var area = MaxRectangleHelper.GetLargestArea(round.Surface);
            Assert.AreEqual(GrillLength / 2, area.Length);
            Assert.AreEqual(GrillWidth / 2, area.Width);
            Assert.AreEqual(GrillLength / 2, area.LengthPosition);
            Assert.AreEqual(GrillWidth / 2, area.WidthPosition);
        }

        [TestMethod]
        public void SmallMealInMiddle()
        {
            var round = GetNewGrillRound();
            round.AddMeat(GetMeat(1, 1), false, GrillLength / 2, GrillWidth / 2);
            var area = MaxRectangleHelper.GetLargestArea(round.Surface);
            Assert.AreEqual(GrillLength / 2, area.Length);
            Assert.AreEqual(GrillWidth, area.Width);
            Assert.AreEqual(0, area.LengthPosition);
            Assert.AreEqual(0, area.WidthPosition);
        }

        [TestMethod]
        public void SmallMealInMiddleInHalfOfHalfSurface()
        {
            var round = GetNewGrillRound();
            round.AddMeat(GetMeat(GrillLength / 2, GrillWidth), false, 0, 0);
            round.AddMeat(GetMeat(GrillLength / 2, GrillWidth / 2), false, GrillLength / 2, 0);
            round.AddMeat(GetMeat(1, 1), false, GrillLength / 2 + 3, GrillWidth / 2 + 3);
            var area = MaxRectangleHelper.GetLargestArea(round.Surface);           
            Assert.AreEqual(10, area.Length);
            Assert.AreEqual(11, area.Width);
            Assert.AreEqual(10, area.LengthPosition);
            Assert.AreEqual(16, area.WidthPosition);
        }

        [TestMethod]
        public void Small1x1AvailableSpace()
        {
            var round = GetNewGrillRound();
            round.AddMeat(GetMeat(GrillLength - 2, GrillWidth), false, 2, 0);
            round.AddMeat(GetMeat(1, GrillWidth), false, 0, 0);
            round.AddMeat(GetMeat(1, GrillWidth - 2), false, 1, 0);
            round.AddMeat(GetMeat(1, 1), false, 1, GrillWidth - 1);
            var area = MaxRectangleHelper.GetLargestArea(round.Surface);
            Assert.AreEqual(1, area.Length);
            Assert.AreEqual(1, area.Width);
            Assert.AreEqual(1, area.LengthPosition);
            Assert.AreEqual(GrillWidth - 2, area.WidthPosition);
        }

        #region Private methods

        private GrillRound GetNewGrillRound()
        {
            return new GrillRound(1, GrillLength, GrillWidth);
        }

        private Meat GetMeat(int length, int width)
        {
            return new Meat()
            {
                Name = "",
                Menu = new Menu(),
                Length = length,
                Width = width,
                Quantity = 1,
                PrepearedQuantity = 0,
                Time = 8,
            };
        }

        #endregion
    }
}
