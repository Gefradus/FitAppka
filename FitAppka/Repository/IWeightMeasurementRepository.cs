using FitAppka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAppka.Repository
{
    public interface IWeightMeasurementRepository
    {
        WeightMeasurement GetWeightMeasurement(int id);
        IEnumerable<WeightMeasurement> GetAllWeightMeasurements();
        WeightMeasurement Add(WeightMeasurement weightMeasurement);
        WeightMeasurement Update(WeightMeasurement weightMeasurement);
        WeightMeasurement Delete(int id);
    }
}
