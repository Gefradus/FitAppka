using System;

namespace FitAppka.Service
{
    public interface IHomePageService
    {
        public void Home(DateTime daySelected);
        public bool IsItFirstLaunch();
        public void EditMeal(int mealID, int grammage);
        public void DeleteMeal(int mealID);
        public void AddMeal(int inWhich, int dayID, int grammage, int productID);
        public void AddWater(int dayID, string form);
        public void EditWater(int dayID, string form);
        public double SumAllKcalInDay(DateTime daySelected);
        public double SumAllProteinsInDay(DateTime daySelected);
        public double SumAllCarbsInDay(DateTime daySelected);
        public double SumAllFatsInDay(DateTime daySelected);
        public decimal Round(double var);
        public int CountPercentageOfTarget(double var, int? target);
        public string DateFormat(DateTime daySelected);
    }
}
