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

        public ProductManageServiceImpl(IDayRepository dayRepository, IWebHostEnvironment hostEnvironment, 
            IProductRepository productRepository, IClientRepository clientRepository, IClientManageService clientManageService)
        {
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


        private string CreatePathToPhoto(CreateProductModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string folder = Path.Combine(_hostEnvironment.WebRootPath, "photos");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(folder, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            return uniqueFileName;
        }

        public void CreateProductFromModel(CreateProductModel model)
        {
            _productRepository.Add(new Product()
            {
                ClientId = _clientRepository.GetLoggedInClientId(),
                VisibleToAll = _clientRepository.IsLoggedInClientAdmin(),
                ProductName = model.ProductName,
                PhotoPath = CreatePathToPhoto(model),
                Calories = (double)model.Calories,
                Proteins = (double)model.Proteins,
                Fats = (double)model.Fats,
                Carbohydrates = (double)model.Carbohydrates,
                VitaminA = model.VitaminA,
                VitaminC = model.VitaminC,
                VitaminD = model.VitaminD,
                VitaminK = model.VitaminK,
                VitaminE = model.VitaminE,
                VitaminB1 = model.VitaminB1,
                VitaminB2 = model.VitaminB2,
                VitaminB6 = model.VitaminB6,
                Biotin = model.Biotin,
                VitaminB12 = model.VitaminB12,
                VitaminPp = model.VitaminPp,
                Zinc = model.Zinc,
                Phosphorus = model.Phosphorus,
                Iodine = model.Iodine,
                Magnesium = model.Magnesium,
                Copper = model.Copper,
                Potassium = model.Potassium,
                Selenium = model.Selenium,
                Sodium = model.Sodium,
                Calcium = model.Calcium,
                Iron = model.Iron
            });
        }

        public void UpdateProduct(CreateProductModel model, int id, int addOrEditPhoto)
        {
            Product product = _productRepository.GetProduct(id);
            if(_clientManageService.HasUserAccessToProduct(product.ProductId))
            {
                product.ProductName = model.ProductName;
                product.Calories = (double)model.Calories;
                product.Proteins = (double)model.Proteins;
                product.Fats = (double)model.Fats;
                product.Carbohydrates = (double)model.Carbohydrates;
                product.VitaminA = model.VitaminA;
                product.VitaminC = model.VitaminC;
                product.VitaminD = model.VitaminD;
                product.VitaminK = model.VitaminK;
                product.VitaminE = model.VitaminE;
                product.VitaminB1 = model.VitaminB1;
                product.VitaminB2 = model.VitaminB2;
                product.VitaminB5 = model.VitaminB5;
                product.VitaminB6 = model.VitaminB6;
                product.Biotin = model.Biotin;
                product.VitaminB12 = model.VitaminB12;
                product.VitaminPp = model.VitaminPp;
                product.Zinc = model.Zinc;
                product.Phosphorus = model.Phosphorus;
                product.Iodine = model.Iodine;
                product.FolicAcid = model.FolicAcid;
                product.Magnesium = model.Magnesium;
                product.Copper = model.Copper;
                product.Potassium = model.Potassium;
                product.Selenium = model.Selenium;
                product.Sodium = model.Sodium;
                product.Calcium = model.Calcium;
                product.Iron = model.Iron;

                if (addOrEditPhoto == 0 || CreatePathToPhoto(model) != null) {
                    product.PhotoPath = CreatePathToPhoto(model);
                }

                _productRepository.Update(product);
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
