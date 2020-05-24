using BarbecueChef.GrillRounds;
using BarbecueChef.MaxRectangle;
using BarbecueChef.MeatChoosers;
using System.Collections.Generic;
using System.Linq;

namespace BarbecueChef.Grills
{
    public class Grill
    {
        public int Length { get; private set; }
        public int Width { get; private set; }     
        public GrillRound CurrentRound { get; private set; }
        public Queue<GrillRound> FinishedRounds { get; }
        public bool IsMenuOnWait => _menus.Count > 0;
        public long AllMeatsSurfaceArea => _meats.Sum(m => m.Area * (long)m.Quantity);

        #region Private fields

        private List<Menu> _menus;
        private List<Meat> _meats;
        private int _numberOfRounds = 0;
        private int _menuSortOrder = 0;
        private IMeatChooserStrategy _strategy;
        private IMaxRectangle _maxRectangle;

        #endregion

        public Grill(int length, int width, IMeatChooserStrategy strategy, IMaxRectangle maxRectangle)
        {
            Length = length;
            Width = width;
            _strategy = strategy;
            _maxRectangle = maxRectangle;

            CurrentRound = new GrillRound(++_numberOfRounds, Length, Width);
            FinishedRounds = new Queue<GrillRound>();
            _menus = new List<Menu>();
            _meats = new List<Meat>();
        }

        public void AddMenu(Menu menu)
        {
            menu.SortOrder = ++_menuSortOrder;
            _menus.Add(menu);
            if (menu.Items.Count > 0)
            {
                _meats.AddRange(menu.Items);
            }
        }

        public void AddMenus(IEnumerable<Menu> menus)
        {
            foreach (var menu in menus)
            {
                AddMenu(menu);
            }
        }

        /// <summary>
        /// Fill the grill with best fit meat.
        /// </summary>
        public virtual void FillCurrentRound()
        {
            Meat meat;
            do
            {
                var area = _maxRectangle.GetLargestRectangle(CurrentRound.Surface);
                meat = _strategy.GetNextMeat(_meats, area.Length, area.Width, out bool rotate);
                if (meat != null)
                {
                    CurrentRound.AddMeat(meat, rotate, area.LengthPosition, area.WidthPosition);
                }
            } while (meat != null);
        }

        /// <summary>
        /// Store CurrentRound and create a new one.
        /// </summary>
        public void CreateNewRound()
        {
            FinishedRounds.Enqueue(CurrentRound);
            CurrentRound = new GrillRound(++_numberOfRounds, Length, Width);
        }

        /// <summary>
        /// For the given time calculate if the meat is finished.
        /// </summary>
        /// <param name="seconds">Elapsed time</param>
        public void TimeElapsed(int seconds)
        {
            CurrentRound.TimeElapsed(seconds);
        }

        /// <summary>
        /// Gets finished Menus from the grill menu list.
        /// </summary>
        /// <returns>Finished menus</returns>
        public List<Menu> GetFinishedMenus()
        {
            List<Menu> finishedMenus = new List<Menu>();
            foreach (var menu in _menus)
            {
                if (menu.Finished)
                {
                    finishedMenus.Add(menu);
                }
            }
            foreach (var menu in finishedMenus)
            {
                _menus.Remove(menu);
                foreach (var item in menu.Items)
                {
                    _meats.Remove(item);
                }
            }
            return finishedMenus;
        }
    }
}
