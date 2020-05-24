using BarbecueChef.MeatChoosers;
using BarbecueChef.Grills;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BarbecueChef.Tests
{
    [TestClass]
    public class MeatChoserTests
    {
        [TestMethod]
        public void EveryChooserReturnsMeat()
        {
            var meats = GetMeatsWithMaxLengthAndWidth8();
            var meatChoosers = GetMeatChoosers();
            foreach (var chooser in meatChoosers)
            {                
                var meal = chooser.GetNextMeat(meats, int.MaxValue, int.MaxValue, out bool rotate);
                Assert.IsNotNull(meal);
            }
        }

        [TestMethod]
        public void IfThereISNoMeatReturnNull()
        {
            var meats = new List<Meat>();
            var meatChoosers = GetMeatChoosers();
            foreach (var chooser in meatChoosers)
            {
                var meal = chooser.GetNextMeat(meats, int.MaxValue, int.MaxValue, out bool rotate);
                Assert.IsNull(meal);
            }
        }

        [TestMethod]
        public void MaxArea()
        {
            var meats = GetMeatsWithMaxLengthAndWidth8();
            var matchMeal = GetMeat(12, 10);
            meats.Add(matchMeal);
            var meal = new LargestAreaMeatChooserStrategy().GetNextMeat(meats, 20, 10, out bool rotate);
            Assert.AreEqual(matchMeal, meal);
        }

        [TestMethod]
        public void MaxAreaWithRotate()
        {
            var meats = GetMeatsWithMaxLengthAndWidth8();
            var matchMeal = GetMeat(12, 10);
            meats.Add(matchMeal);
            var matchWithRotate = GetMeat(10, 20);
            meats.Add(matchWithRotate);
            var meal = new LargestAreaMeatChooserStrategy().GetNextMeat(meats, 20, 10, out bool rotate);
            Assert.AreEqual(matchWithRotate, meal);
        }

        [TestMethod]
        public void MaxLength()
        {
            var meats = GetMeatsWithMaxLengthAndWidth8();
            var matchMeal = GetMeat(12, 1);
            meats.Add(matchMeal);
            var meal = new LargestLengthMeatChooserStrategy().GetNextMeat(meats, 20, 10, out bool rotate);
            Assert.AreEqual(matchMeal, meal);
        }

        [TestMethod]
        public void MaxLengthRotate()
        {
            var meats = GetMeatsWithMaxLengthAndWidth8();
            var matchMeal = GetMeat(12, 1);
            meats.Add(matchMeal);
            var matchWithRotate = GetMeat(1, 20);
            meats.Add(matchWithRotate);
            var meal = new LargestLengthMeatChooserStrategy().GetNextMeat(meats, 20, 10, out bool rotate);
            Assert.AreEqual(matchWithRotate, meal);
        }

        [TestMethod]
        public void MaxWidth()
        {
            var meats = GetMeatsWithMaxLengthAndWidth8();
            var matchMeal = GetMeat(1, 12);
            meats.Add(matchMeal);
            var meal = new LargestWidthMeatChooserStrategy().GetNextMeat(meats, 10, 20, out bool rotate);
            Assert.AreEqual(matchMeal, meal);
        }

        [TestMethod]
        public void MaxWidthRotate()
        {
            var meats = GetMeatsWithMaxLengthAndWidth8();
            var matchMeal = GetMeat(1, 12);
            meats.Add(matchMeal);
            var matchWithRotate = GetMeat(20, 1);
            meats.Add(matchWithRotate);
            var meal = new LargestWidthMeatChooserStrategy().GetNextMeat(meats, 10, 20, out bool rotate);
            Assert.AreEqual(matchWithRotate, meal);
        }

        #region Private methods

        private List<Meat> GetMeatsWithMaxLengthAndWidth8()
        {
            return new List<Meat>()
            {
                GetMeat(1, 5),
                GetMeat(5, 1),
                GetMeat(2, 2),
                GetMeat(3, 3),
                GetMeat(4, 4),
                GetMeat(1, 2),
                GetMeat(2, 1),
                GetMeat(1, 8),
                GetMeat(8, 1),
                GetMeat(1, 1),
            };
        }

        private Meat GetMeat(int length, int width)
        {
            return new Meat()
            {
                Name = $"{length}x{width}",
                Menu = new Menu(),
                Length = length,
                Width = width,
                Quantity = 1,
                PrepearedQuantity = 0,
                Time = 8,
            };
        }

        private List<IMeatChooserStrategy> GetMeatChoosers()
        {
            return new List<IMeatChooserStrategy>()
            {
                new LargestAreaMeatChooserStrategy(),
                new LargestLengthMeatChooserStrategy(),
                new LargestWidthMeatChooserStrategy(),
            };
        }

        #endregion
    }
}
