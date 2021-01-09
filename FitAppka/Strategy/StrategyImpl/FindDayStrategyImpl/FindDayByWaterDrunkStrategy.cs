using FitnessApp.Models;
using FitnessApp.Repository;
using FitnessApp.Strategy.StrategyInterface;
using System.Collections.Generic;

namespace FitnessApp.Strategy.StrategyImpl.FindDayStrategyImpl
{
    public class FindDayByWaterDrunkStrategy : IFindDayStrategy
    {
        public IDayRepository DayRepository { private get; set; }

        public List<DayDTO> FindDays(FindDayDTO dto)
        {
            var list = new List<DayDTO>();
            foreach (var day in DayRepository.GetLoggedInClientDays())
            {
                if((day.WaterDrunk <= dto.From || dto.From == null) && (day.WaterDrunk >= dto.To || dto.To == null) &&
                day.Date >= DateConverter.ConvertToDateTimeFrom(dto.DateFrom) && day.Date <= DateConverter.ConvertToDateTimeTo(dto.DateTo))
                {
                    list.Add(new DayDTO() {
                        Date = day.Date.GetValueOrDefault(),
                        DataSearchedFor = day.WaterDrunk.GetValueOrDefault(),
                        DayId = day.DayId
                    });
                }
            }

            return list;
        }
    }
}
