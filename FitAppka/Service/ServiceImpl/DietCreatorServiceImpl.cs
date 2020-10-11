using AutoMapper;
using FitAppka.Models;
using FitAppka.Models.DTO.DietCreatorDTO;
using FitAppka.Repository;
using FitAppka.Repository.RepoInterface;
using FitAppka.Service.ServiceInterface;
using FitAppka.Strategy.StrategyDictionary;
using FitAppka.Strategy.StrategyEnum;
using FitAppka.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class DietCreatorServiceImpl : IDietCreatorService
    {
        public IDietRepository DietRepository { get; private set; }
        private readonly IProductRepository _productRepository;
        private readonly IDietProductRepository _dietProductRepository;
        private readonly IContentRootPathHandlerService _contentRootService;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public DietCreatorServiceImpl(IDietRepository dietRepository, IMapper mapper, IContentRootPathHandlerService contentRootService,
            IDietProductRepository dietProductRepository, IProductRepository productRepository, IClientRepository clientRepository)
        {
            DietRepository = dietRepository;
            _clientRepository = clientRepository;
            _contentRootService = contentRootService;
            _dietProductRepository = dietProductRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public ActiveDietDTO GetActiveDiet(int dayOfWeek)
        {
            new DayOfWeekDietStrategyDictionary<IDayOfWeekDietStrategy>(this, _mapper).
                TryGetValue((DayOfWeek)dayOfWeek, out IDayOfWeekDietStrategy mapValue);

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
                int clientId = _clientRepository.GetLoggedInClientId();
                Diet diet = _mapper.Map<DietDTO, Diet>(dietDTO);
                diet.ClientId = clientId;
                SetDietsToNotActiveIfDaysConflict(diet);
                DietRepository.Add(diet);

                foreach (var item in products)
                {
                    DietProduct product = _mapper.Map<DietProductDTO, DietProduct>(item);
                    product.DietId = diet.DietId;
                    _dietProductRepository.Add(product);
                }

                return true;
            }
            return false;
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

        
    }
}
