using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Repository
{
    public interface IMealRepository
    {
        Meal GetMeal(int id);
        IEnumerable<Meal> GetAllMeals();
        IEnumerable<Meal> GetMealsOfTheDay(int dayId, int whichMealOfTheDay);
        Meal Add(Meal meal);
        Meal Update(Meal meal);
        Meal Delete(int id);
    }
}
