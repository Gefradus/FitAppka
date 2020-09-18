using FitAppka.Model;

namespace FitAppka.Service
{
    public interface IMeasurementsService
    {
        bool AddOrUpdateMeasurements(short weight, int waist);
        bool UpdateMeasurements(int id, short weight, int waist);
        bool DeleteMeasurement(int id);
        double EstimateBodyFatLevel(short weight, int waist);
    }
}
