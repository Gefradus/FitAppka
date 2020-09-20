using AutoMapper;
using FitAppka.Models;
using FitAppka.Repository;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;

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

        public string MealName(int inWhichMeal)
        {
            if (inWhichMeal == 1) { return "Śniadanie"; }
            if (inWhichMeal == 2) { return "II śniadanie"; }
            if (inWhichMeal == 3) { return "Obiad"; }
            if (inWhichMeal == 4) { return "Deser"; }
            if (inWhichMeal == 5) { return "Przekąska"; }
            return "Kolacja";
        }

        public string DayPattern(int dayID)
        {
            DateTime daySelected = _dayRepository.GetDayDateTime(dayID);
            string month = "";

            if (daySelected.Month == 1) { month = "sty"; }
            if (daySelected.Month == 2) { month = "lut"; }
            if (daySelected.Month == 3) { month = "mar"; }
            if (daySelected.Month == 4) { month = "kwi"; }
            if (daySelected.Month == 5) { month = "maj"; }
            if (daySelected.Month == 6) { month = "czer"; }
            if (daySelected.Month == 7) { month = "lip"; }
            if (daySelected.Month == 8) { month = "sie"; }
            if (daySelected.Month == 9) { month = "wrz"; }
            if (daySelected.Month == 10) { month = "paź"; }
            if (daySelected.Month == 11) { month = "lis"; }
            if (daySelected.Month == 12) { month = "gru"; }

            if (daySelected == DateTime.Now.Date) {
                return "Dzisiaj";
            }
            else if (daySelected == DateTime.Now.Date.AddDays(-1)) {
                return "Wczoraj";
            }
            else if (daySelected == DateTime.Now.Date.AddDays(1)) {
                return "Jutro";
            }
            else {
                return daySelected.Day + " " + month;
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

        public void CreateProductFromModel(ProductDTO productDTO)
        {
            Product product = _mapper.Map<ProductDTO, Product>(productDTO);
            product.ClientId = _clientRepository.GetLoggedInClientId();
            product.VisibleToAll = _clientRepository.IsLoggedInClientAdmin();
            product.PhotoPath = CreatePathToPhoto(productDTO);
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

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public Product Add(Product product)
        {
            return _productRepository.Add(product);
        }

        public Product Update(Product product)
        {
            return _productRepository.Update(product);
        }

        public Product Delete(int id)
        {
            return _productRepository.Delete(id);
        }
    }
}
