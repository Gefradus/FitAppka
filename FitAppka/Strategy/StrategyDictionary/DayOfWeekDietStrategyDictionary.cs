using AutoMapper;
using FitAppka.Service.ServiceImpl;
using FitAppka.Strategy.StrategyEnum;
using FitAppka.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl;
using System;
using System.Collections.Generic;

namespace FitAppka.Strategy.StrategyDictionary
{
    public class DayOfWeekDietStrategyDictionary<T> : Dictionary<DayOfWeekDietStrategyEnum, T>
    {
        private readonly DietCreatorServiceImpl serviceImpl;
        private readonly IMapper _mapper;

        public DayOfWeekDietStrategyDictionary(DietCreatorServiceImpl service, IMapper mapper)
        {
            serviceImpl = service;
            _mapper = mapper;
            Add(DayOfWeekDietStrategyEnum.Monday, ConvertToT(new MondayDietStrategy()));
            Add(DayOfWeekDietStrategyEnum.Tuesday, ConvertToT(new TuesdayDietStrategy()));
            Add(DayOfWeekDietStrategyEnum.Wednesday, ConvertToT(new WednesdayDietStrategy()));
            Add(DayOfWeekDietStrategyEnum.Thursday, ConvertToT(new ThursdayDietStrategy()));
            Add(DayOfWeekDietStrategyEnum.Friday, ConvertToT(new FridayDietStrategy()));
            Add(DayOfWeekDietStrategyEnum.Saturday, ConvertToT(new SaturdayDietStrategy()));
            Add(DayOfWeekDietStrategyEnum.Sunday, ConvertToT(new SundayDietStrategy()));
        }

        private T ConvertToT<Strategy>(Strategy s)
        {
            return (T)Convert.ChangeType(_mapper.Map(serviceImpl, s), typeof(Strategy));
        }
    }
}
