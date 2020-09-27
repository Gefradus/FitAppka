using AutoMapper;
using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Strategy.StrategyDictionary;
using FitAppka.Strategy.StrategyEnum;
using FitAppka.Strategy.StrategyInterface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitAppka.Service.ServiceImpl
{
    public class FindDayServiceImpl : IFindDayService
    {
        public IMealRepository MealRepository { get; set; }
        public IDayRepository DayRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        private readonly IMapper _mapper;

        public FindDayServiceImpl(IMealRepository mealRepository, IDayRepository dayRepository, IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper;
            ProductRepository = productRepository;
            DayRepository = dayRepository;
            MealRepository = mealRepository;
        }

        public List<DaysAndProductsDTO> FindDays(FindDayDTO findDayDTO)
        {
            new FindDayStrategyDictionary<IFindDayStrategy>(this, _mapper).
                TryGetValue((FindDayStrategyEnum)findDayDTO.FindBy, out IFindDayStrategy strategy);

            return new DaysAndProductsDTO()
            {
                Days = strategy.FindDays(findDayDTO),
                
            };
        }

        public List<SelectListItem> CreateProductsList(int productID)
        {
            throw new System.NotImplementedException();
        }
    }
}
