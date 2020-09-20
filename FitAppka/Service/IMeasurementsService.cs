namespace FitAppka.Service
{
    public interface IMeasurementsService
    {
        void AddMeasurements(short weight, int? waist);
        bool UpdateMeasurements(int id, short weight, int? waist);
        bool DeleteMeasurement(int id);
        double EstimateBodyFatLevel(short weight, int? waist);
    }
}
