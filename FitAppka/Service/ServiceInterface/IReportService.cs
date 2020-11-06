using FitAppka.Models.DTO;
using System.Collections.Generic;

namespace FitAppka.Service.ServiceInterface
{
    public interface IReportService
    {
        public List<DaySummaryDTO> GetDays();
        public DaySummaryWithDetailsDTO GetDayWithDetails();
    }
}
