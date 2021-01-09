using FitnessApp.Models;

namespace FitnessApp.Strategy.StrategyInterface
{
    public interface IChartTypeStrategy
    {
        ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo);
    }
}
