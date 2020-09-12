using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class CardioTrainingType
    {
        public CardioTrainingType()
        {
            CardioTraining = new HashSet<CardioTraining>();
        }

        [Key]
        [Column("CardioTrainingType_ID")]
        public int CardioTrainingTypeId { get; set; }
        [Column("Client_ID")]
        public int? ClientId { get; set; }
        [Column("Training_name")]
        [StringLength(50)]
        public string TrainingName { get; set; }
        [Column("Kcal_per_min")]
        public int? KcalPerMin { get; set; }
        [Column("Visible_to_all")]
        public bool? VisibleToAll { get; set; }
        [ForeignKey(nameof(ClientId))]
        [InverseProperty("CardioTrainingType")]
        public virtual Client Client { get; set; }
        [InverseProperty("CardioTrainingType")]
        public virtual ICollection<CardioTraining> CardioTraining { get; set; }
    }
}
