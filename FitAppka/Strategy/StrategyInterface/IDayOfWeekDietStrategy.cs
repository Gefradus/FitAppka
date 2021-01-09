using FitnessApp.Models;

namespace FitnessApp.Strategy.StrategyInterface
{
    public interface IDayOfWeekDietStrategy
    {
        public Diet GetActiveDiet();
    }
}
