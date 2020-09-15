using FitAppka.Model;

namespace FitAppka.Service
{
    public interface IMeasurementsService
    {
        public bool AddOrUpdateMeasurement(WeightMeasurement weightMeasurement);
        public bool UpdateMeasurement(WeightMeasurement weightMeasurement);
        public bool DeleteMeasurement(int id);
    }
}
