using FitAppka.Models;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Repository.RepIfaceImpl
{
    public class SQLMealRepository : IMealRepository
    {
        private readonly FitAppContext _context;

        public SQLMealRepository(FitAppContext context)
        {
            _context = context;
        }

        public Meal Add(Meal meal)
        {
            _context.Add(meal);
            _context.SaveChanges();
            return meal;
        }

        public Meal Delete(int id)
        {
            Meal meal = GetMeal(id);

            if (meal != null)
            {
                _context.Meal.Remove(meal);
                _context.SaveChanges();
            }

            return meal;
        }

        public IEnumerable<Meal> GetAllMeals()
        {
            return _context.Meal.ToList();
        }

        public Meal GetMeal(int id)
        {
            return _context.Meal.Find(id);
        }

        public Meal Update(Meal meal)
        {
            _context.Update(meal);
            _context.SaveChanges();
            return meal;
        }
    }
}
