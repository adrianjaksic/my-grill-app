using BarbecueChef.Exceptions;
using BarbecueChef.GrillRounds;
using BarbecueChef.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BarbecueChef.Tests
{
    [TestClass]
    public class GrillRoundTests
    {
        private const int GrillLength = 20;
        private const int GrillWidth = 30;
        private const string MeatTestName = "Test meat";
        private const int MeatTime = 8;

        [TestMethod]
        public void TestMeatAdding()
        {
            var round = GetNewGrillRound();
            const int meatLength = 2;
            const int meatWidth = 2;
            var meat = GetMeat(meatLength, meatWidth, 1);
            Assert.AreEqual(true, round.AllFinished);
            Assert.AreEqual(0, round.Items.Count);
            Assert.AreEqual(0, round.GrillUsedArea);
            round.AddMeat(meat, false, 0, 0);
            Assert.AreEqual(false, round.AllFinished);
            Assert.AreEqual(1, round.Items.Count);
            Assert.AreEqual(meatLength * meatWidth, round.GrillUsedArea);
        }

        [TestMethod]
        [ExpectedException(typeof(GrillRoundAvailabilityException))]
        public void TestMeatAddingToTheSamePlace()
        {
            var round = GetNewGrillRound();
            const int meatLength = 2;
            const int meatWidth = 2;
            var meat = GetMeat(meatLength, meatWidth, 2);
            round.AddMeat(meat, false, 0, 0);
            round.AddMeat(meat, false, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(GrillRoundAvailabilityException))]
        public void TestWrongMeatSize()
        {
            var round = GetNewGrillRound();
            var meat = GetMeat(GrillWidth, GrillLength, 2);
            round.AddMeat(meat, false, 0, 0);
        }

        [TestMethod]
        public void TestMeatAddingWithRotation()
        {
            var round = GetNewGrillRound();
            var meat = GetMeat(GrillWidth, GrillLength, 2);
            round.AddMeat(meat, true, 0, 0);
            Assert.AreEqual(false, round.AllFinished);
            Assert.AreEqual(GrillWidth * GrillLength, round.GrillUsedArea);
        }

        [TestMethod]
        public void FillWholeGrillWithElapsedTimeToFinish()
        {
            var round = GetNewGrillRound();
            const int meatLength = 1;
            const int meatWidth = 1;
            var meat = GetMeat(meatLength, meatWidth, round.GrillSurfaceArea);
            for (int i = 0; i < GrillLength; i++)
            {
                for (int j = 0; j < GrillWidth; j++)
                {
                    round.AddMeat(meat, false, i, j);
                }
            }
            Assert.AreEqual(false, round.AllFinished);
            Assert.AreEqual(GrillLength * GrillWidth, round.GrillUsedArea);
            round.TimeElapsed(MeatTime);
            Assert.AreEqual(true, round.AllFinished);
        }

        #region Private methods

        private GrillRound GetNewGrillRound()
        {
            return new GrillRound(1, GrillLength, GrillWidth);
        }

        private Meat GetMeat(int length, int width, int quantity)
        {
            return new Meat()
            {
                Name = MeatTestName,
                Menu = new Menu(),
                Length = length,
                Width = width,
                Quantity = quantity,
                PrepearedQuantity = 0,
                Time = MeatTime,
            };
        }

        #endregion
    }
}
