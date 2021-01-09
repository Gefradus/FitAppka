using AutoMapper;
using FitnessApp.Service.ServiceImpl;
using FitnessApp.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl;
using System;
using System.Collections.Generic;

namespace FitnessApp.Strategy.StrategyDictionary
{
    public class DayOfWeekDietStrategyDictionary<T> : Dictionary<DayOfWeek, T>
    {
        private readonly DietCreatorServiceImpl serviceImpl;
        private readonly IMapper _mapper;

        public DayOfWeekDietStrategyDictionary(DietCreatorServiceImpl service, IMapper mapper)
        {
            serviceImpl = service;
            _mapper = mapper;
            Add(DayOfWeek.Monday, ConvertToT(new MondayDietStrategy()));
            Add(DayOfWeek.Tuesday, ConvertToT(new TuesdayDietStrategy()));
            Add(DayOfWeek.Wednesday, ConvertToT(new WednesdayDietStrategy()));
            Add(DayOfWeek.Thursday, ConvertToT(new ThursdayDietStrategy()));
            Add(DayOfWeek.Friday, ConvertToT(new FridayDietStrategy()));
            Add(DayOfWeek.Saturday, ConvertToT(new SaturdayDietStrategy()));
            Add(DayOfWeek.Sunday, ConvertToT(new SundayDietStrategy()));
        }

        private T ConvertToT<Strategy>(Strategy s)
        {
            return (T)Convert.ChangeType(_mapper.Map(serviceImpl, s), typeof(Strategy));
        }
    }
}
