using FitAppka.Models.DTO.DaySummaryDTO;

namespace FitAppka.Service.ServiceInterface
{
    public interface IReportService
    {
        public DaysSummaryDTO GetMonthSummary(int dayId);
        public DaySummaryWithDetailsDTO GetDayWithDetails(int dayId);
    }
}
