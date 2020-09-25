using FitAppka.Models;

namespace FitAppka.Strategy.StrategyInterface
{
    public interface IChartTypeStrategy
    {
        ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo);
    }
}
