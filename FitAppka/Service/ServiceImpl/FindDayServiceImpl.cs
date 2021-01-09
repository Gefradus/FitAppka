using AutoMapper;
using FitnessApp.Models;
using FitnessApp.Repository;
using FitnessApp.Strategy.StrategyDictionary;
using FitnessApp.Strategy.StrategyEnum;
using FitnessApp.Strategy.StrategyInterface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitnessApp.Service.ServiceImpl
{
    public class FindDayServiceImpl : IFindDayService
    {
        public IMealRepository MealRepository { get; private set; }
        public IDayRepository DayRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        private readonly IMapper _mapper;

        public FindDayServiceImpl(IMealRepository mealRepository, IDayRepository dayRepository, IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper;
            ProductRepository = productRepository;
            DayRepository = dayRepository;
            MealRepository = mealRepository;
        }

        public FindDayDTO FindDays(FindDayDTO dto)
        {
            new FindDayStrategyDictionary<IFindDayStrategy>(this, _mapper)
                .TryGetValue((FindDayStrategyEnum)dto.FindBy, out IFindDayStrategy strategy);

            dto.Days = strategy.FindDays(dto);
            dto.Products = CreateProductsList(dto.ProductId);
            return dto;
        }

        public List<SelectListItem> CreateProductsList(int productId)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ProductRepository.GetAccessedToLoggedInClientProducts())
            {
                SelectListItem selectListItem = new SelectListItem() {
                    Text = item.ProductName + ", " + (int)item.Calories + "kcal",
                    Value = item.ProductId.ToString()
                };

                if (item.ProductId == productId) {
                    selectListItem.Selected = true;
                }

                list.Add(selectListItem);
            }

            return list;
        }
    }
}
