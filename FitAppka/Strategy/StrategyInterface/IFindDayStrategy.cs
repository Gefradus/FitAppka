using FitnessApp.Models;
using System.Collections.Generic;

namespace FitnessApp.Strategy.StrategyInterface
{
    public interface IFindDayStrategy
    {
        public List<DayDTO> FindDays(FindDayDTO findDayDTO);
    }
}
