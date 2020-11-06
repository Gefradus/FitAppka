using AutoMapper;
using FitAppka.Models;
using FitAppka.Models.DTO.DietCreatorDTO;
using FitAppka.Models.DTO.EditDietDTO;
using FitAppka.Repository;
using FitAppka.Repository.RepoInterface;
using FitAppka.Service.ServiceInterface;
using FitAppka.Strategy.StrategyDictionary;
using FitAppka.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class DietCreatorServiceImpl : IDietCreatorService
    {
        public IDietRepository DietRepository { get; private set; }
        public IClientRepository ClientRepository { get; private set; }
        private readonly IClientManageService _clientManageService;
        private readonly IProductRepository _productRepository;
        private readonly IDietProductRepository _dietProductRepository;
        private readonly IContentRootPathHandlerService _contentRootService;
        private readonly IMapper _mapper;

        public DietCreatorServiceImpl(IDietRepository dietRepository, IMapper mapper, 
            IContentRootPathHandlerService contentRootService, IClientManageService clientManageService,
            IDietProductRepository dietProductRepository, IProductRepository productRepository, IClientRepository clientRepository)
        {
            DietRepository = dietRepository;
            _clientManageService = clientManageService;
            ClientRepository = clientRepository;
            _contentRootService = contentRootService;
            _dietProductRepository = dietProductRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public ActiveDietDTO GetActiveDiet(int dayOfWeek, bool wasDayChanged)
        {
            new DayOfWeekDietStrategyDictionary<IDayOfWeekDietStrategy>(this, _mapper).
                TryGetValue(wasDayChanged ? (DayOfWeek)dayOfWeek : DateTime.Now.DayOfWeek, out IDayOfWeekDietStrategy mapValue);

            DietDTO dietDTO = _mapper.Map<Diet, DietDTO>(mapValue.GetActiveDiet());
            
            if(dietDTO == null) {
                return null;
            } 
            
            var dietProducts = _dietProductRepository.GetDietProducts(dietDTO.DietId);
            return new ActiveDietDTO() {
                Diet = dietDTO,
                Products = MapProductsToDietProductsDTO(MapDietProductsToDTO(dietProducts)),
                CaloriesSum = CountCaloriesSum(dietProducts),
                ProteinsSum = CountProteinsSum(dietProducts),
                FatsSum = CountFatsSum(dietProducts),
                CarbohydratesSum = CountCarbsSum(dietProducts)
            };   
        }


        public int CountCaloriesSum(List<DietProduct> list)
        {
            double kcal = 0;
            foreach(var product in list) {
                kcal += (_productRepository.GetProduct(product.ProductId).Calories * product.Grammage / 100);
            }
            return (int)kcal;
        }

        private double CountProteinsSum(List<DietProduct> list)
        {
            double proteins = 0;
            foreach (var product in list) {
                proteins += (double)Math.Round((decimal)
                (_productRepository.GetProduct(product.ProductId).Proteins * product.Grammage / 100), 1, MidpointRounding.AwayFromZero);
            }
            return (double)Math.Round((decimal)proteins, 1, MidpointRounding.AwayFromZero);
        }

        private double CountFatsSum(List<DietProduct> list)
        {
            double fats = 0;
            foreach (var product in list) {
                fats += (double)Math.Round((decimal)
                (_productRepository.GetProduct(product.ProductId).Fats * product.Grammage / 100), 1, MidpointRounding.AwayFromZero);
            }
            return (double)Math.Round((decimal)fats, 1, MidpointRounding.AwayFromZero);
        }

        private double CountCarbsSum(List<DietProduct> list)
        {
            double carbs = 0;
            foreach (var product in list) {
                carbs += (double)Math.Round((decimal)
                (_productRepository.GetProduct(product.ProductId).Carbohydrates * product.Grammage / 100), 1, MidpointRounding.AwayFromZero);
            }
            return (double)Math.Round((decimal)carbs, 1, MidpointRounding.AwayFromZero); 
        }

        

        public List<DietProductDTO> MapProductsToDietProductsDTO(List<DietProductDTO> productsDTO)
        {
            List<DietProductDTO> list = new List<DietProductDTO>();
            foreach(var dietProduct in productsDTO) {
                list.Add(_mapper.Map(_productRepository.GetProduct(dietProduct.ProductId), dietProduct));
            }
            return list;
        }

        public List<DietProductDTO> MapDietProductsToDTO(List<DietProduct> dietProducts)
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

        public CreateDietDTO SearchProducts(string search, bool wasSearched)
        {
            return new CreateDietDTO() {
                SearchProducts = SearchOrGetProducts(search),
                RootPath = _contentRootService.GetContentRootFileName(),
                WasSearched = wasSearched
            };
        }

        public CreateDietDTO AddProduct(List<DietProductDTO> addedProducts, int productId, int grammage, bool wasSearched)
        {
            DietProductDTO dto = new DietProductDTO() { Grammage = grammage, TempId = FindMaxValue(addedProducts) };
            addedProducts.Add(_mapper.Map(_productRepository.GetProduct(productId), dto));

            return new CreateDietDTO() {
                SearchProducts = SearchOrGetProducts(null),
                AddedProducts = addedProducts,
                RootPath = _contentRootService.GetContentRootFileName(),
                WasSearched = wasSearched
            };
        }

        private int FindMaxValue(List<DietProductDTO> list)
        {
            if(list.Count > 0) {
              return list.OrderByDescending(p => p.TempId).First().TempId + 1;
            }
            return 0;
        }

        public CreateDietDTO DeleteProduct(List<DietProductDTO> addedProducts, int tempId)
        {
            for(int i = 0; i < addedProducts.Count; i++) {
                if(addedProducts.ElementAt(i).TempId == tempId) {
                    addedProducts.RemoveAt(i);
                    break;
                }
            }

            return new CreateDietDTO()
            {
                SearchProducts = SearchOrGetProducts(null),
                AddedProducts = addedProducts,
                RootPath = _contentRootService.GetContentRootFileName(),
                WasSearched = false
            };
        }

        private List<SearchProductDTO> SearchOrGetProducts(string search)
        {
            var searchProducts = new List<SearchProductDTO>();
            foreach (var item in _productRepository.GetAccessedToLoggedInClientProducts().
                Where(p => string.IsNullOrEmpty(search) || p.ProductName.ToLower().Contains(search.ToLower())).ToList())
            {
                searchProducts.Add(_mapper.Map<Product, SearchProductDTO>(item));
            }
            return searchProducts;
        }

        public bool CreateDiet(List<DietProductDTO> products, DietDTO dietDTO, bool overriding)
        {
            if(overriding || CheckIfDietsHaveNoConflict(dietDTO))
            {
                int clientId = _clientManageService.GetLoggedInClientId();
                Diet diet = _mapper.Map<DietDTO, Diet>(dietDTO);
                diet.ClientId = clientId;
                diet.IsDeleted = false;
                SetDietsToNotActiveIfDaysConflict(diet);
                DietRepository.Add(diet);

                foreach (var item in products) {
                    DietProduct product = _mapper.Map<DietProductDTO, DietProduct>(item);
                    product.DietId = diet.DietId;
                    _dietProductRepository.Add(product);
                }

                return true;
            }
            return false;
        }

        public bool EditDiet(List<DietProductDTO> products, DietDTO dietDTO, bool overriding)
        {
            if (_clientManageService.HasUserAccessToDiet(dietDTO.DietId)) {
                if (overriding || CheckIfDietsHaveNoConflict(dietDTO)) 
                {
                    Diet diet = UpdateDiet(dietDTO);
                    RemoveAllDietProductsAssignedToDiet(diet.DietId);
                    foreach (var item in products)
                    {
                        DietProduct product = _mapper.Map<DietProductDTO, DietProduct>(item);
                        product.DietId = diet.DietId;
                        _dietProductRepository.Add(product);
                    }

                    return true;
                }
            }
            return false;
        }

        private Diet UpdateDiet(DietDTO dietDTO)
        {
            Diet diet = _mapper.Map(dietDTO, DietRepository.GetDiet(dietDTO.DietId));
            SetDietsToNotActiveIfDaysConflict(diet);
            diet.Active = dietDTO.Active;
            return DietRepository.Update(diet);
        }

        private void RemoveAllDietProductsAssignedToDiet(int dietId)
        {
            foreach(var item in _dietProductRepository.GetAllDietProducts())
            {
                if(item.DietId == dietId)
                {
                    _dietProductRepository.Delete(item.DietProductId);
                }
            }
        }

        private bool CheckIfDietsHaveNoConflict(DietDTO diet) {
            if (diet.Active) {
                foreach(var item in DietRepository.GetLoggedInClientDiets()) {
                    if(item.Active) {
                        if(item.Monday && diet.Monday || item.Tuesday && diet.Tuesday || item.Wednesday && diet.Wednesday 
                            || item.Thursday && diet.Thursday || item.Friday && diet.Friday 
                            || item.Saturday && diet.Saturday || item.Sunday && diet.Sunday) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void SetDietsToNotActiveIfDaysConflict(Diet diet)
        {
            if (diet.Active) {
                foreach(var item in DietRepository.GetLoggedInClientDiets()) {
                    if(item.Active) {
                        if(item.Monday && diet.Monday || item.Tuesday && diet.Tuesday || item.Wednesday && diet.Wednesday 
                            || item.Thursday && diet.Thursday || item.Friday && diet.Friday 
                            || item.Saturday && diet.Saturday || item.Sunday && diet.Sunday) 
                        {
                            item.Active = false;
                            DietRepository.Update(item);
                        }
                    }
                }
            }
        }

        public List<ActiveDietDTO> GetLoggedInClientActiveDiets()
        {
            var list = new List<DietDTO>();
            foreach(var item in DietRepository.GetLoggedInClientDiets()){
                list.Add(_mapper.Map<Diet, DietDTO>(item));
            }

            var listOfActiveDiets = new List<ActiveDietDTO>();
            foreach(var item in list)
            {
                var dietProducts = _dietProductRepository.GetDietProducts(item.DietId);
                listOfActiveDiets.Add(new ActiveDietDTO()
                {
                    Diet = item,
                    Products = MapProductsToDietProductsDTO(MapDietProductsToDTO(dietProducts)),
                    CaloriesSum = CountCaloriesSum(dietProducts)
                });
            }
            return SortListOfActiveDiets(listOfActiveDiets);
        }

        private List<ActiveDietDTO> SortListOfActiveDiets(List<ActiveDietDTO> list)
        {
            return list.OrderBy(l => !l.Diet.Active).ThenBy(l => !l.Diet.Monday).ThenBy(l => !l.Diet.Tuesday).ThenBy(l => !l.Diet.Wednesday)
                .ThenBy(l => !l.Diet.Thursday).ThenBy(l => !l.Diet.Friday).ThenBy(l => !l.Diet.Saturday).ThenBy(l => !l.Diet.Sunday)
                .ThenBy(l => l.CaloriesSum).ToList();
        }


        public EditDietDTO EditDietSearchProduct(int id, string search, bool wasSearched)
        {
            return new EditDietDTO()
            {
                AddedProducts = MapProductsToDietProductsDTO(MapDietProductsToDTO(_dietProductRepository.GetDietProducts(id))),
                EditedDiet = _mapper.Map<Diet, DietDTO>(DietRepository.GetDiet(id)),
                SearchProducts = SearchOrGetProducts(search),
                RootPath = _contentRootService.GetContentRootFileName(),
                WasSearched = wasSearched
            };
        }

        public bool DeleteDiet(int id)
        {
            try {
                if (_clientManageService.HasUserAccessToDiet(id)) {
                    DietRepository.Delete(id);
                    return true;
                } 
                return false;
            }
            catch {
                return false;
            }
        }
        
    }
}
