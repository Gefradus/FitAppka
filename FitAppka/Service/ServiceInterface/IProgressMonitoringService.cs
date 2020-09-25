using FitAppka.Models;

namespace FitAppka.Service
{
    public interface IProgressMonitoringService
    {
        ProgressMonitoringDTO GetProgressMonitoringDTO(string dateFrom, string dateTo, int chartType);
    }
}
