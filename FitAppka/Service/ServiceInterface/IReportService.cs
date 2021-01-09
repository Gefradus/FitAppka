using FitnessApp.Models.DTO.DaySummaryDTO;

namespace FitnessApp.Service.ServiceInterface
{
    public interface IReportService
    {
        public DaysSummaryDTO GetMonthSummary(int dayId);
        public DaySummaryWithDetailsDTO GetDayWithDetails(int dayId);
    }
}
