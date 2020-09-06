using Microsoft.Extensions.Primitives;
using System;

namespace FitAppka.Service
{
    public interface IHomePageService
    {
        public void Home(DateTime daySelected);
        public bool IsItFirstLaunch();
        public void EditMeal(int id, int grammage);
        public void DeleteMeal(int id);
        public void AddMeal(int inWhich, int dayID, int grammage, int productID);
        public void AddWater(int dayID, StringValues form);
        public void EditWater(int dayID, StringValues form);
        public double SumAllKcalInDay(DateTime daySelected);
        public double SumAllProteinsInDay(DateTime daySelected);
        public double SumAllCarbsInDay(DateTime daySelected);
        public double SumAllFatsInDay(DateTime daySelected);
        public int CountPercentageOfTarget(double var, int? target);
        public string DateFormat(DateTime daySelected);
        public decimal CountCalories(int whichMeal, DateTime daySelected, int clientID);
    }
}
