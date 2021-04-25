namespace FitnessApp.Models.DTO
{
    public class StrengthTrainingDTO
    {
        public int Id { get; set; }
        public string StrengthTrainingName { get; set; }
        public short Repetitions { get; set; }
        public short Sets { get; set; }
        public short? Load { get; set; }
    }
}
