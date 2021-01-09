using FitnessApp.Models;
using System.Collections.Generic;

namespace FitnessApp.Repository
{
    public interface IMealRepository
    {
        Meal GetMeal(int id);
        IEnumerable<Meal> GetAllMeals();
        IEnumerable<Meal> GetMealsOfTheDay(int dayId, int whichMealOfTheDay);
        List<Meal> GetAllDayMeals(int dayId);
        Meal Add(Meal meal);
        Meal Update(Meal meal);
        Meal Delete(int id);
    }
}
