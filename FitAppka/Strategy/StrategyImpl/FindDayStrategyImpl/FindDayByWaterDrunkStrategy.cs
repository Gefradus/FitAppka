using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Strategy.StrategyInterface;
using System.Collections.Generic;

namespace FitAppka.Strategy.StrategyImpl.FindDayStrategyImpl
{
    public class FindDayByWaterDrunkStrategy : IFindDayStrategy
    {
        public IDayRepository DayRepository { get; set; }

        public List<DayDTO> FindDays(FindDayDTO dto)
        {
            var list = new List<DayDTO>();
            foreach (var day in DayRepository.GetLoggedInClientDays())
            {
                if(day.WaterDrunk <= dto.From && day.WaterDrunk >= dto.To &&
                day.Date <= DateConverter.ConvertToDateTimeFrom(dto.DateFrom) && day.Date >= DateConverter.ConvertToDateTimeTo(dto.DateTo))
                {
                    list.Add(new DayDTO(){
                        Date = day.Date.GetValueOrDefault(),
                        DataSearchedFor = day.WaterDrunk.GetValueOrDefault()
                    });
                }
            }

            return list;
        }
    }
}
