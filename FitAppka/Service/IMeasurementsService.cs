using FitAppka.Model;

namespace FitAppka.Service
{
    public interface IMeasurementsService
    {
        public bool AddOrUpdateMeasurements(short weight, int waist);
        public WeightMeasurement UpdateWeightMeasurement(WeightMeasurement weightMeasurement);
        public bool DeleteMeasurement(int id);
        public double EstimateBodyFatLevel(short weight, int waist);
    }
}
