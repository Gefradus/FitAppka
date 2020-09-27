using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Strategy.StrategyInterface
{
    public interface IFindDayStrategy
    {
        public List<DayDTO> FindDays(FindDayDTO findDayDTO);
    }
}
