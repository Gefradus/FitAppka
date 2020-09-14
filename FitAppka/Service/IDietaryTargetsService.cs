using FitAppka.Models;
using System;
using System.Collections.Generic;

namespace FitAppka.Service
{
    public interface IDietaryTargetsService
    {
        public int CountCalorieTarget(int demand, short? changeGoal, double pace);
        public int CountCaloricDemand(bool? sex, DateTime? birthDate, int? growth, double? weight, double activity);
        public int CountProteinTarget(double? weight, int kcalTarget, short? activity);
        public int CountFatTarget(int kcalTarget);
        public int CountCarbsTarget(int kcalTarget, int proteinsTarget, int fatsTarget);
        public int CountAge(DateTime birthDate);
        public short GetLastWeightMeasurement();
        public List<int> GetListOfDaysIDFromToday(Client client);
        public void SetTargetsInDaysFromToday(Client client);
    }
}
