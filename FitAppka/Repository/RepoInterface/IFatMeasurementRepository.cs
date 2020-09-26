using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Repository
{
    public interface IFatMeasurementRepository
    {
        FatMeasurement GetFatMeasurement(int id);
        IEnumerable<FatMeasurement> GetAllFatMeasurements();
        IEnumerable<FatMeasurement> GetLoggedInClientFatMeasurements();
        FatMeasurement Add(FatMeasurement fatMeasurement);
        FatMeasurement Update(FatMeasurement fatMeasurement);
        FatMeasurement Delete(int id);
    }
}
