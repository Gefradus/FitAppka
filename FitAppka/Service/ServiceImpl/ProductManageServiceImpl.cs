using AutoMapper;
using FitAppka.Models;
using FitAppka.Models.Enum;
using FitAppka.Repository;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FitAppka.Service.ServiceImpl
{
    public class ProductManageServiceImpl : IProductManageService
    {
        private readonly IDayRepository _dayRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IClientManageService _clientManageService;
        private readonly IMapper _mapper;

        public ProductManageServiceImpl(IDayRepository dayRepository, IWebHostEnvironment hostEnvironment, IMapper mapper,
            IProductRepository productRepository, IClientRepository clientRepository, IClientManageService clientManageService)
        {
            _mapper = mapper;
            _clientManageService = clientManageService;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _hostEnvironment = hostEnvironment;
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

        public async Task<List<ProductDTO>> SearchProduct(string search, bool onlyUserItem)
        {
            return await _mapper.Map<Task<List<Product>>, Task<List<ProductDTO>>>(onlyUserItem ? _productRepository.GetLoggedInClientProducts() : _productRepository.SearchProducts(search));
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

                _productRepository.Update(_mapper.Map(product, _mapper.Map<ProductDTO, Product>(productDTO)));
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
    }
}
