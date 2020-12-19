using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class Goals
    {
        public Goals()
        {
            Client = new HashSet<Client>();
            Day = new HashSet<Day>();
        }

        [Key]
        [Column("Goals_ID")]
        public int GoalsId { get; set; }
        [Column("Client_ID")]
        public int? ClientId { get; set; }
        [Column("Day_ID")]
        public int? DayId { get; set; }
        [Column("Kcal_burned")]
        public int? KcalBurned { get; set; }
        [Column("Training_time")]
        public int? TrainingTime { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("GoalsNavigation")]
        public virtual Client ClientNavigation { get; set; }
        [ForeignKey(nameof(DayId))]
        [InverseProperty("GoalsNavigation")]
        public virtual Day DayNavigation { get; set; }
        [InverseProperty("Goals")]
        public virtual ICollection<Client> Client { get; set; }
        [InverseProperty("Goals")]
        public virtual ICollection<Day> Day { get; set; }
    }
}
