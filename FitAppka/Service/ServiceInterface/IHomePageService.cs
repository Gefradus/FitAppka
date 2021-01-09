using System;
using System.Collections.Generic;

namespace FitnessApp.Service
{
    public interface IHomePageService
    {
        public void EditMeal(int id, int grammage);
        public void DeleteMeal(int id);
        public DateTime AddMeal(int inWhich, int dayID, int grammage, int productID);
        public void AddWater(int dayID, int addedWater);
        public void EditWater(int dayID, int editedWater);
        public bool IsItFirstLaunch();
        public int CountPercentageOfTarget(double var, double target);
        public decimal Round(double number);
        public decimal CountCalories(int whichMeal, DateTime daySelected);
        public double SumAllListItems(List<double> list);
        public double SumAllKcalInDay(DateTime daySelected);
        public double SumAllProteinsInDay(DateTime daySelected);
        public double SumAllCarbsInDay(DateTime daySelected);
        public double SumAllFatsInDay(DateTime daySelected);
        public string DateFormat(DateTime daySelected);
        
    }
}
