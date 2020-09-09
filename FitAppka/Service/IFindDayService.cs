using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitAppka.Service
{
    public interface IFindDayService
    {
        public List<int> FindDays(int from, int to, int productID, int searchType, int clientID);
        public List<SelectListItem> CreateProductsList(int productID);
        public List<int?> WaterInDays(List<int> daysList);
        public List<int> GrammageInDays(List<int> daysList, int productID);
        public List<decimal> CaloriesInDays(List<int> daysList);
        public bool WheterDayFound(List<int> daysList);
    }
}
