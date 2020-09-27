using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Strategy.StrategyInterface;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Strategy.StrategyImpl.FindDayStrategyImpl
{
    public class FindDayByProductConsumedStrategy : IFindDayStrategy
    {
        public IDayRepository DayRepository { get; set; }
        public IMealRepository MealRepository { get; set; }


        public List<DayDTO> FindDays(FindDayDTO dto)
        {
            var list = new List<DayDTO>();
            var meals = MealRepository.GetAllMeals().Where(p => p.ProductId == dto.ProductId);
            foreach (var day in DayRepository.GetLoggedInClientDays())
            {
                int sumOfGrammages = 0;
                foreach (var meal in meals.Where(p => p.DayId == day.DayId)) {
                    sumOfGrammages += meal.Grammage;
                }

                if ((sumOfGrammages >= dto.From || dto.From == null) && (sumOfGrammages <= dto.To || dto.To == null) && sumOfGrammages != 0 &&
                    day.Date >= DateConverter.ConvertToDateTimeFrom(dto.DateFrom) && day.Date <= DateConverter.ConvertToDateTimeTo(dto.DateTo))
                {
                    list.Add(new DayDTO() {
                        Date = day.Date.GetValueOrDefault(),
                        DataSearchedFor = sumOfGrammages,
                        DayId = day.DayId
                    });
                }
            }

            return list;
        }
    }
}
