using FitnessApp.Models;
using System.Collections.Generic;

namespace FitnessApp.Repository
{
    public interface IWeightMeasurementRepository
    {
        WeightMeasurement GetWeightMeasurement(int id);
        IEnumerable<WeightMeasurement> GetAllWeightMeasurements();
        List<WeightMeasurement> GetLoggedInClientWeightMeasurements();
        List<WeightMeasurement> GetClientWeightMeasurements(int clientId);
        WeightMeasurement Add(WeightMeasurement weightMeasurement);
        WeightMeasurement Update(WeightMeasurement weightMeasurement);
        WeightMeasurement Delete(int id);
        double GetLastLoggedInClientWeight();
        double GetLastClientWeight(int clientId);
    }
}
