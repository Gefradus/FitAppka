using FitAppka.Models;
namespace FitAppka.Service.ServiceInterface
{
    public interface IDietCreatorService
    {
        public DietCreatorDTO GetActiveDiet(int dayOfWeek);
    }
}
