using FitAppka.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitAppka.Service
{
    public interface IFindDayService
    {
        public List<DaysAndProductsDTO> FindDays(FindDayDTO findDayDTO);
        public List<SelectListItem> CreateProductsList(int productID);
    }
}
