using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.Models.DTO
{
    public class HomeDTO
    {
        public List<Meal> Meals { get; set; }
        public decimal BreakfastKcal { get; set; }
        public decimal LunchKcal { get; set; }
        public decimal DinnerKcal { get; set; }
        public decimal DessertKcal { get; set; }
        public decimal SnackKcal { get; set; }
        public decimal SupperKcal { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public bool Dessert { get; set; }
        public bool Snack { get; set; }
        public bool Supper { get; set; }
        public int IsAdmin { get; set; }
        public DateTime Day { get; set; }
        public string Date { get; set; }
        public string Datepick { get; set; }
        public string Path { get; set; }
        public int ClientID { get; set; }
        public int DayID { get; set; }
        public int? Water { get; set; }

    }
}
