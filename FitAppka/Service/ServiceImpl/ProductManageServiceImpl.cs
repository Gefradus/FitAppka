using AutoMapper;
using FitnessApp.Models;
using FitnessApp.Models.DTO;
using FitnessApp.Models.Enum;
using FitnessApp.Repository;
using FitnessApp.Repository.RepoInterface;
using FitnessApp.Service.ServiceInterface;
using Microsoft.AspNetCore.Hosting;
using X.PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FitnessApp.Service.ServiceImpl
{
    public class ProductManageServiceImpl : IProductManageService
    {
        private readonly IDayRepository _dayRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IClientManageService _clientManageService;
        private readonly IDietCreatorService _dietCreatorService;
        private readonly IDietProductRepository _dietProductRepository;
        private readonly IContentRootPathHandlerService _contentRootService;
        private readonly IMealRepository _mealRepository;
        private readonly IMapper _mapper;

        public ProductManageServiceImpl(IDayRepository dayRepository, IWebHostEnvironment hostEnvironment, IMapper mapper, 
            IDietCreatorService dietCreatorService, IMealRepository mealRepository, IProductRepository productRepository, 
            IClientRepository clientRepository, IClientManageService clientManageService, IDietProductRepository dietProductRepository,
            IContentRootPathHandlerService contentRootService)
        {
            _mapper = mapper;
            _dietCreatorService = dietCreatorService;
            _dietProductRepository = dietProductRepository;
            _clientManageService = clientManageService;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _mealRepository = mealRepository;
            _hostEnvironment = hostEnvironment;
            _contentRootService = contentRootService;
            _dayRepository = dayRepository;
        }

        public string MealName(int atWhichMealOfTheDay)
        {
            return ((MealOfTheDayEnum)atWhichMealOfTheDay).ToString().Replace("_"," ");
        }

        public string DayPattern(int dayID)
        {
            DateTime daySelected = _dayRepository.GetDayDateTime(dayID);
            DateTime today = DateTime.Now.Date;

            if (daySelected == today) {
                return "Dzisiaj";
            }
            else if (daySelected == today.AddDays(-1)) {
                return "Wczoraj";
            }
            else if (daySelected == today.AddDays(1)) {
                return "Jutro";
            }
            else {
                return daySelected.Day + " " + (MonthEnum)daySelected.Month;
            } 
        }

        private string CreatePathToPhoto(ProductDTO productDTO)
        {
            string uniqueFileName = null;
            if (productDTO.Photo != null)
            {
                string folder = Path.Combine(_hostEnvironment.WebRootPath, "photos");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + productDTO.Photo.FileName;
                string filePath = Path.Combine(folder, uniqueFileName);
                productDTO.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            return uniqueFileName;
        }

        public IPagedList<ProductDTO> SearchProduct(string search, bool onlyUserItem, int dayId, bool onlyFromDiet, int? page)
        {
            List<ProductDTO> list = _mapper.Map<List<Product>, List<ProductDTO>>(onlyUserItem ? _productRepository.GetLoggedInClientProducts() :
                onlyFromDiet ? GetProductsFromDiet(dayId) : _productRepository.SearchProducts(search));

            return (onlyFromDiet ? CheckIfEaten(list, dayId) : list.OrderBy(p => p.ProductName).ToList()).ToPagedList(page ?? 1, 15);
        }



        private List<ProductDTO> CheckIfEaten(List<ProductDTO> list, int dayId)
        {
            var activeDiet = _dietCreatorService.GetActiveDiet((int)_dayRepository.GetDay(dayId).Date.GetValueOrDefault().DayOfWeek, true);
            if (activeDiet != null)
            {
                foreach (var meal in _mealRepository.GetAllDayMeals(dayId))
                {
                    foreach (var product in list)
                    {
                        foreach (var diet in _dietProductRepository.GetDietProducts(activeDiet.Diet.DietId))
                        {
                            if (diet.ProductId == product.ProductId && product.ProductId == meal.ProductId
                                && diet.Grammage <= _mealRepository.GetAllDayMeals(dayId).Where(m => m.ProductId == product.ProductId).Sum(m => m.Grammage))
                            {
                                product.Eaten = true;
                            }
                        }
                    }
                }
            }
            return list.OrderBy(p => p.Eaten).ThenBy(p => p.ProductName).ToList();
        }


        private List<Product> GetProductsFromDiet(int dayId)
        {
            var diet = _dietCreatorService.GetActiveDiet((int)_dayRepository.GetDay(dayId).Date.GetValueOrDefault().DayOfWeek, true);
            
            var list = new List<Product>();
            var idList = new List<int>();
            if (diet != null) {
                foreach (var item in _dietProductRepository.GetDietProducts(diet.Diet.DietId))
                {
                    if (!idList.Contains(item.ProductId)) {
                        idList.Add(item.ProductId);
                        list.Add(_productRepository.GetProduct(item.ProductId));
                    }
                }
            }
            return list;
        }

        public void CreateProductFromModel(ProductDTO productDTO)
        {
            Product product = _mapper.Map<ProductDTO, Product>(productDTO);
            product.ClientId = _clientRepository.GetLoggedInClientId();
            product.VisibleToAll = _clientRepository.IsLoggedInClientAdmin();
            product.PhotoPath = CreatePathToPhoto(productDTO);
            product.IsDeleted = false;
            _productRepository.Add(product);
        }

        public void UpdateProduct(ProductDTO productDTO, int id, int addOrEditPhoto)
        {
            Product product = _productRepository.GetProductAsNoTracking(id);
            if(_clientManageService.HasUserAccessToProduct(product.ProductId))
            {
                string photoPath = CreatePathToPhoto(productDTO);
                if (addOrEditPhoto == 0 || photoPath != null) {
                    product.PhotoPath = photoPath;
                }

                product = _mapper.Map(product, _mapper.Map<ProductDTO, Product>(productDTO));
                product.IsDeleted = false;
                _productRepository.Update(product);
            }            
        }

        public Product GetProduct(int id)
        {
            return _productRepository.GetProduct(id);
        }

        public Product Delete(int id)
        {
            return _productRepository.Delete(id);
        }

        public SearchProductViewDTO Dto(SearchProductDTO dto)
        {
            dto.WereSearched = dto.Search != null;
            dto.MealName = MealName(dto.AtWhich);
            dto.ContentRootPath = _contentRootService.GetContentRootFileName();
            dto.Day = DayPattern(dto.DayId);

            return new SearchProductViewDTO()
            {
                Products = SearchProduct(dto.Search, dto.OnlyUserItem, dto.DayId, dto.OnlyFromDiet, dto.Page),
                SearchDTO = dto
            };
        }

        public AdminProductDTO AdminDto(string search, bool onlyUserItem, int dayId, bool onlyFromDiet, int? page)
        {
            return new AdminProductDTO()
            {
                Products = SearchProduct(search, onlyUserItem, dayId, onlyFromDiet, page),
                Search = search
            };
        }
    }
}
