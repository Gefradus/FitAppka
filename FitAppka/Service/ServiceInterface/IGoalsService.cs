using FitAppka.Models;
using System;
using System.Collections.Generic;

namespace FitAppka.Service
{
    public interface IGoalsService
    {
        int CountCalorieTarget(int demand, short? changeGoal, double pace);
        int CountCaloricDemand(bool? sex, DateTime? birthDate, int? growth, double? weight, double activity);
        int CountProteinTarget(double? weight, int kcalTarget, short? activity);
        int CountFatTarget(int kcalTarget);
        int CountCarbsTarget(int kcalTarget, int proteinsTarget, int fatsTarget);
        double ActivityLevel(short? activity);
        int CountAge(DateTime birthDate);
        List<int> GetListOfDaysIDFromToday(int clientId);
        void UpdateGoalsInDaysFromToday();
        int CaloriesBurnedInDay(int dayID);
        void UpdateGoals(GoalsDTO createGoalsModel);
        Goals AddOrUpdateClientGoals(Client client, int calorieTarget, int proteinTarget, int fatTarget, int carbsTarget);
        Goals SetClientGoalsIfAutoDietaryGoals(bool autoDietaryGoals);
        Goals GetDayGoals(int dayId);
        GoalsDTO MapClientGoalsToCreateGoalsModel();
    }
}
