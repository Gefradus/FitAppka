using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class Product
    {
        public Product()
        {
            DietProduct = new HashSet<DietProduct>();
            Meal = new HashSet<Meal>();
        }

        [Key]
        [Column("Product_ID")]
        public int ProductId { get; set; }
        [Column("Client_ID")]
        public int ClientId { get; set; }
        [Required]
        [Column("Product_name")]
        [StringLength(50)]
        public string ProductName { get; set; }
        [Column("Photo_path")]
        [StringLength(512)]
        public string PhotoPath { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }

        [Column("Visible_to_all")]
        public bool VisibleToAll { get; set; }
        [Column("Vitamin_A")]
        public double? VitaminA { get; set; }
        [Column("Vitamin_C")]
        public double? VitaminC { get; set; }
        [Column("Vitamin_D")]
        public double? VitaminD { get; set; }
        [Column("Vitamin_K")]
        public double? VitaminK { get; set; }
        [Column("Vitamin_E")]
        public double? VitaminE { get; set; }
        [Column("Vitamin_B1")]
        public double? VitaminB1 { get; set; }
        [Column("Vitamin_B2")]
        public double? VitaminB2 { get; set; }
        [Column("Vitamin_B5")]
        public double? VitaminB5 { get; set; }
        [Column("Vitamin_B6")]
        public double? VitaminB6 { get; set; }
        public double? Biotin { get; set; }
        [Column("Folic_acid")]
        public double? FolicAcid { get; set; }
        [Column("Vitamin_B12")]
        public double? VitaminB12 { get; set; }
        [Column("Vitamin_PP")]
        public double? VitaminPp { get; set; }
        public double? Zinc { get; set; }
        public double? Phosphorus { get; set; }
        public double? Iodine { get; set; }
        public double? Magnesium { get; set; }
        public double? Copper { get; set; }
        public double? Potassium { get; set; }
        public double? Selenium { get; set; }
        public double? Sodium { get; set; }
        public double? Calcium { get; set; }
        public double? Iron { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("Product")]
        public virtual Client Client { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<DietProduct> DietProduct { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Meal> Meal { get; set; }
    }
}
