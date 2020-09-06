using FitAppka.Models;
using FitAppka.Repository;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace FitAppka.Service.ServiceImpl
{
    public class ProductManageServiceImpl : IProductManageService
    {
        private readonly IDayRepository _dayRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductRepository _productRepository;

        public ProductManageServiceImpl(IDayRepository dayRepository, IWebHostEnvironment hostEnvironment, IProductRepository productRepository)
        {
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
    }
}
