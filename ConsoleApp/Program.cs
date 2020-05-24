using BarbecueChef.Grills;
using BarbecueChef.MaxRectangle;
using BarbecueChef.MeatChoosers;
using ConsoleApp.Interfaces;
using ConsoleApp.Services;
using System;

namespace ConsoleApp
{
    class Program
    {
        //Best strategies: LargestWidthMeatChooserStrategy-31, LargestWidthMenuPriorityMeatChooserStrategy-32, LargestLengthMeatChooserStrategy-36, LargestAreaMeatChooserStrategy-31
        private static readonly IMeatChooserStrategy MeatChooserStrategy = new LargestWidthMenuPriorityMeatChooserStrategy();
        private static readonly IMaxRectangle MaxRectangle = new MaxRectangleHistogram();

        private static readonly IMenuService menuService = new MenuRequestService();
        private const int TimeElapsedFixedForAllMeals = 480;
        static void Main(string[] args)
        {
            var grill = new Grill(20, 30, MeatChooserStrategy, MaxRectangle);
            var menus = menuService.GetMenus();
            grill.AddMenus(menus);

            Console.WriteLine($"Number of menus: {menus.Count}, All meat size: {grill.AllMeatsSurfaceArea}, Best fit number of rounds: {Math.Floor((decimal)grill.AllMeatsSurfaceArea / grill.CurrentRound.GrillSurfaceArea)}");
            do
            {
                grill.FillCurrentRound();
                Console.Write($"Round {grill.CurrentRound.RoundNumber} menus ({grill.CurrentRound.GrillSurfaceArea}/{grill.CurrentRound.GrillUsedArea}): ");
                grill.TimeElapsed(TimeElapsedFixedForAllMeals);
                var roundMenus = grill.GetFinishedMenus();
                if (roundMenus.Count > 0)
                {
                    foreach (var menu in roundMenus)
                    {
                        Console.Write($"{menu.Name},");
                    }
                }
                else
                {
                    Console.Write("-");
                }
                Console.WriteLine();
                grill.CreateNewRound();
            } while (grill.IsMenuOnWait);

            Console.Write("To end application press ant key...");
            Console.ReadKey();
        }
    }
}
