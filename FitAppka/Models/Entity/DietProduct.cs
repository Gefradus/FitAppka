using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    public partial class DietProduct
    {
        [Key]
        [Column("DietProduct_ID")]
        public int DietProductId { get; set; }
        [Column("Diet_ID")]
        public int DietId { get; set; }
        [Column("Product_ID")]
        public int ProductId { get; set; }
        public int Grammage { get; set; }

        [ForeignKey(nameof(DietId))]
        [InverseProperty("DietProduct")]
        public virtual Diet Diet { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty("DietProduct")]
        public virtual Product Product { get; set; }
    }
}
