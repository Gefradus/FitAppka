using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Repository
{
    public interface IWeightMeasurementRepository
    {
        WeightMeasurement GetWeightMeasurement(int id);
        IEnumerable<WeightMeasurement> GetAllWeightMeasurements();
        IEnumerable<WeightMeasurement> GetClientsWeightMeasurements(Client client);
        WeightMeasurement Add(WeightMeasurement weightMeasurement);
        WeightMeasurement Update(WeightMeasurement weightMeasurement);
        WeightMeasurement Delete(int id);
    }
}
