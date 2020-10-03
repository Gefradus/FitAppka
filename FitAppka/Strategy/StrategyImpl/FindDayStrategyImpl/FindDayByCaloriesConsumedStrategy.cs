using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Strategy.StrategyInterface;
using System.Collections.Generic;

namespace FitAppka.Strategy.StrategyImpl.FindDayStrategyImpl
{
    public class FindDayByCaloriesConsumedStrategy : IFindDayStrategy
    {
        public IDayRepository DayRepository { private get; set; }
        public IMealRepository MealRepository { private get; set; }


        public List<DayDTO> FindDays(FindDayDTO dto) {

            var list = new List<DayDTO>();
            foreach (var day in DayRepository.GetLoggedInClientDays()) 
            {
                double sumOfCaloriesInDay = 0;
                foreach (var meal in MealRepository.GetAllMeals()) {
                    if (meal.DayId == day.DayId) {
                        sumOfCaloriesInDay += meal.Calories;
                    }
                }

                if((sumOfCaloriesInDay >= dto.From || dto.From == null) && (sumOfCaloriesInDay <= dto.To || dto.To == null) && 
                    day.Date >= DateConverter.ConvertToDateTimeFrom(dto.DateFrom) && day.Date <= DateConverter.ConvertToDateTimeTo(dto.DateTo)) 
                {
                    list.Add(new DayDTO() {
                        Date = day.Date.GetValueOrDefault(),
                        DataSearchedFor = (int)sumOfCaloriesInDay,
                        DayId = day.DayId
                    });
                }
            }

            return list;
        }


        
    }
}
