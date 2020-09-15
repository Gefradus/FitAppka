using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Model
{
    public partial class StrengthTrainingType
    {
        public StrengthTrainingType()
        {
            StrengthTraining = new HashSet<StrengthTraining>();
        }

        [Key]
        [Column("StrengthTrainingType_ID")]
        public int StrengthTrainingTypeId { get; set; }
        [Column("Client_ID")]
        public int? ClientId { get; set; }
        [Required]
        [Column("Training_name")]
        [StringLength(50)]
        public string TrainingName { get; set; }
        [Column("Visible_to_all")]
        public bool VisibleToAll { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("StrengthTrainingType")]
        public virtual Client Client { get; set; }
        [InverseProperty("StrengthTrainingType")]
        public virtual ICollection<StrengthTraining> StrengthTraining { get; set; }
    }
}
