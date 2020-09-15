using FitAppka.Model;
using System.Threading.Tasks;

namespace FitAppka.Service
{
    public interface IClientManageService
    {
        public Task AddNewClient(RegisterModel model);
        public string GetClientLoginFromModel(Client model);
        public bool CheckIfPassCorrect(Client model);
        public bool HasUserAccessToDay(int dayID);
        public bool HasUserAccessToProduct(int productID);
        public bool HasUserAccessToWeightMeasurement(int id);
    }
}
