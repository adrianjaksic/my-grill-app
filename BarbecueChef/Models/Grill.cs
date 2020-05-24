using BarbecueChef.GrillRounds;
using BarbecueChef.SortingStrategy;
using System.Collections.Generic;

namespace BarbecueChef.Models
{
    public class Grill
    {
        public int Length { get; private set; }
        public int Width { get; private set; }
        public GrillRound CurrentRound { get; private set; }
        public Queue<GrillRound> FinishedRounds { get; }

        private ISortingStrategy _sortingStrategy;
        private List<Menu> _menus;

        public Grill(int length, int width, ISortingStrategy sortingStrategy)
        {
            Length = length;
            Width = width;
            _sortingStrategy = sortingStrategy;

            CurrentRound = new GrillRound(1, Length, Width);
            FinishedRounds = new Queue<GrillRound>();
            _menus = new List<Menu>();
        }

        public void AddMeal(Menu menu)
        {
            _menus.Add(menu);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if no meat left.</returns>
        public bool FillCurrentRound()
        {
            return true; // _sortingStrategy.FillCurrentRound(this);
        }

        public void CreateNewRound()
        {
            //FinishedRounds.Enqueue(CurrentRound);
            //CurrentRound = new GrillRound();
        }

        /// <summary>
        /// Gets finished Menus from the grill menu list.
        /// </summary>
        /// <returns>Finished menus</returns>
        public List<Menu> FinishedMenus()
        {
            List<Menu> finishedMenus = new List<Menu>();
            foreach (var menu in _menus)
            {
                if (menu.IsFinished())
                {
                    finishedMenus.Add(menu);
                }
            }
            foreach (var menu in _menus)
            {
                _menus.Remove(menu);
            }
            return finishedMenus;
        }
    }
}
