using AutoMapper;
using FitnessApp.Service.ServiceImpl;
using FitnessApp.Strategy.StrategyEnum;
using FitnessApp.Strategy.StrategyImpl.FindDayStrategyImpl;
using System;
using System.Collections.Generic;

namespace FitnessApp.Strategy.StrategyDictionary
{
    public class FindDayStrategyDictionary<T> : Dictionary<FindDayStrategyEnum, T>
    {
        private readonly FindDayServiceImpl serviceImpl;
        private readonly IMapper _mapper;

        public FindDayStrategyDictionary(FindDayServiceImpl service, IMapper mapper)
        {
            serviceImpl = service;
            _mapper = mapper;
            Add(FindDayStrategyEnum.ByCaloriesConsumed, ConvertToT(new FindDayByCaloriesConsumedStrategy()));
            Add(FindDayStrategyEnum.ByProductConsumed, ConvertToT(new FindDayByProductConsumedStrategy()));
            Add(FindDayStrategyEnum.ByWaterDrunk, ConvertToT(new FindDayByWaterDrunkStrategy()));
        }

        private T ConvertToT<Strategy>(Strategy s)
        {
            return (T) Convert.ChangeType(_mapper.Map(serviceImpl, s), typeof(Strategy));
        }
    }
}
