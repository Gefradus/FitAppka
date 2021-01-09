using FitnessApp.Models;

namespace FitnessApp.Service
{
    public interface IProgressMonitoringService
    {
        ProgressMonitoringDTO GetProgressMonitoringDTO(string dateFrom, string dateTo, int chartType);
    }
}
