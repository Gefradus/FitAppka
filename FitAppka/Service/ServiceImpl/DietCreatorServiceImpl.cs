using AutoMapper;
using FitAppka.Models;
using FitAppka.Models.DTO.DietCreatorDTO;
using FitAppka.Repository;
using FitAppka.Repository.RepoInterface;
using FitAppka.Service.ServiceInterface;
using FitAppka.Strategy.StrategyDictionary;
using FitAppka.Strategy.StrategyEnum;
using FitAppka.Strategy.StrategyInterface;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class DietCreatorServiceImpl : IDietCreatorService
    {
        public IDietRepository DietRepository { get; private set; }
        private readonly IProductRepository _productRepository;
        private readonly IDietProductRepository _dietProductRepository;
        private readonly IMapper _mapper;

        public DietCreatorServiceImpl(IDietRepository dietRepository, IMapper mapper, 
            IDietProductRepository dietProductRepository, IProductRepository productRepository)
        {
            DietRepository = dietRepository;
            _dietProductRepository = dietProductRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public ActiveDietDTO GetActiveDiet(int dayOfWeek)
        {
            new DayOfWeekDietStrategyDictionary<IDayOfWeekDietStrategy>(this, _mapper).
                TryGetValue((DayOfWeekDietStrategyEnum)dayOfWeek, out IDayOfWeekDietStrategy mapValue);

            DietDTO dietDTO = _mapper.Map<Diet, DietDTO>(mapValue.GetActiveDiet());

            return dietDTO == null ? null : new ActiveDietDTO() {
                Diet = dietDTO,
                Products = MapProductsToDietProductsDTO(MapDietProductsToDTO(_dietProductRepository.GetDietProducts(dietDTO.DietId)))
            };
        }


        private List<DietProductDTO> MapProductsToDietProductsDTO(List<DietProductDTO> productsDTO)
        {
            List<DietProductDTO> list = new List<DietProductDTO>();
            foreach(var dietProduct in productsDTO) {
                list.Add(_mapper.Map(_productRepository.GetProduct(dietProduct.ProductId), dietProduct));
            }
            return list;
        }

        private List<DietProductDTO> MapDietProductsToDTO(List<DietProduct> dietProducts)
        {
            var list = new List<DietProductDTO>();
            foreach(var item in dietProducts) {
                list.Add(new DietProductDTO() {
                    Grammage = item.Grammage,
                    ProductId = item.ProductId
                });
            }
            return list;
        }

        public CreateDietDTO SearchProducts(List<DietProductDTO> addedProducts, string search)
        {
            List<Product> products = _productRepository.GetAccessedToLoggedInClientProducts().
                    Where(p => string.IsNullOrEmpty(search) || p.ProductName.ToLower().Contains(search.ToLower())).ToList();

            var searchdto = new List<SearchProductDTO>();
            foreach (var item in products)
            {
                searchdto.Add(new SearchProductDTO() {
                    Calories = item.Calories,
                    Carbohydrates = item.Carbohydrates,
                    ProductId = item.ProductId,
                    Fats = item.Fats,
                    ProductName = item.ProductName,
                    Proteins = item.Proteins
                });
            }

            return new CreateDietDTO(){
                SearchProducts = searchdto,

                AddedProducts = addedProducts
            };
        }

        public CreateDietDTO AddProduct(List<DietProductDTO> addedProducts, int productId, int grammage)
        {
            DietProductDTO dto = new DietProductDTO() { Grammage = grammage };
            addedProducts.Add(_mapper.Map(_productRepository.GetProduct(productId), dto));

            return new CreateDietDTO()
            {
                SearchProducts = new List<SearchProductDTO>(),
                AddedProducts = addedProducts
            };
        }
    }
}
