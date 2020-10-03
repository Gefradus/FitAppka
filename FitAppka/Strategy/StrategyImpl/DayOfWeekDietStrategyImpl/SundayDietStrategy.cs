using System.Linq;
using FitAppka.Models;
using FitAppka.Repository.RepoInterface;
using FitAppka.Strategy.StrategyInterface;

namespace FitAppka.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl
{
    public class SundayDietStrategy : IDayOfWeekDietStrategy
    {
        public IDietRepository DietRepository { private get; set; }

        public Diet GetActiveDiet()
        {
            return DietRepository.GetAllDiets().FirstOrDefault(d => d.Active && d.Sunday);
        }
    }
}
