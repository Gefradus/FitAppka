using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class Diet
    {
        public Diet()
        {
            DietProduct = new HashSet<DietProduct>();
        }

        [Key]
        [Column("Diet_ID")]
        public int DietId { get; set; }
        [Column("Client_ID")]
        public int ClientId { get; set; }
        [Required]
        [Column("Diet_name")]
        [StringLength(50)]
        public string DietName { get; set; }
        public bool Active { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("Diet")]
        public virtual Client Client { get; set; }
        [InverseProperty("Diet")]
        public virtual ICollection<DietProduct> DietProduct { get; set; }
    }
}
