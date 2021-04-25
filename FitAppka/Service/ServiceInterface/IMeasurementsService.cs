using FitnessApp.Models.DTO;
using X.PagedList;

namespace FitnessApp.Service
{
    public interface IMeasurementsService
    {
        void AddMeasurements(double weight, int? waist);
        bool UpdateMeasurements(int id, double weight, int? waist);
        bool DeleteMeasurement(int id);
        double EstimateBodyFatLevel(double weight, int? waist);
        IPagedList<BodyMeasurementsDTO> Dto(int? page);
    }
}
