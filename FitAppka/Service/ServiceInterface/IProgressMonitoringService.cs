using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Service
{
    public interface IProgressMonitoringService
    {
        List<WeightMeasurement> GetWeightMeasurementListFromTo(string dateFrom, string dateTo);
    }
}
