using FitAppka.Models.DTO;
using FitAppka.Models.DTO.DaySummaryDTO;
using System.Collections.Generic;

namespace FitAppka.Service.ServiceInterface
{
    public interface IReportService
    {
        public DaysSummaryDTO GetDaysSummary();
        public DaySummaryWithDetailsDTO GetDayWithDetails(int dayId);
    }
}
